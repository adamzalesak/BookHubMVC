using System.Globalization;
using System.Linq.Expressions;
using BusinessLayer.Exceptions;
using BusinessLayer.Mappers;
using BusinessLayer.Models.Book;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services;

public class BooksService : IBooksService
{
    private readonly BookHubDbContext _dbContext;

    public BooksService(BookHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BookPaginationModel> GetBooksAsync(GetBooksModel parameters)
    {
        var booksQuery = _dbContext.Books.AsQueryable();

        booksQuery = ApplyFilters(parameters, booksQuery);
        booksQuery = ApplyOrderBy(parameters, booksQuery);

        var books = await booksQuery
            .Skip((parameters.Page ?? 0) * (parameters.PageSize ?? 10))
            .Take(parameters.PageSize ?? 10)
            .Where(x => x.IsDeleted == false)
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Prices)
            .Include(b => b.Genres)
            .Select(b => b.MapToBookModel())
            .ToListAsync();

        var totalCount = await booksQuery
            .Where(x => x.IsDeleted == false)
            .CountAsync();

        return new BookPaginationModel()
        {
            Books = books,
            TotalCount = totalCount,
            PageIndex = parameters.Page ?? 0
        };
    }

    private static IQueryable<Book> ApplyFilters(GetBooksModel parameters, IQueryable<Book> booksQuery)
    {
        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            booksQuery = booksQuery.Where(book => book.Name.ToLower().Contains(parameters.Name.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Description))
        {
            booksQuery = booksQuery.Where(book => book.Description.ToLower().Contains(parameters.Description.ToLower()));
        }

        if (parameters.AuthorId is not null)
        {
            booksQuery = booksQuery.Where(book => book.Authors.Any(a => a.Id == parameters.AuthorId));
        }

        if (!string.IsNullOrWhiteSpace(parameters.AuthorName))
        {
            booksQuery = booksQuery.Where(book => book.Authors.Any(a => a.Name.ToLower().Contains(parameters.AuthorName.ToLower())));
        }

        if (parameters.PublisherId is not null)
        {
            booksQuery = booksQuery.Where(book => book.Publisher.Id == parameters.PublisherId);
        }

        if (!string.IsNullOrWhiteSpace(parameters.PublisherName))
        {
            booksQuery = booksQuery.Where(book => book.Publisher.Name.ToLower().Contains(parameters.PublisherName.ToLower()));
        }

        if (parameters.GenreId is not null)
        {
            booksQuery = booksQuery.Where(book => book.Genres.Any(g => g.Id == parameters.GenreId));
        }

        if (!string.IsNullOrWhiteSpace(parameters.GenreName))
        {
            booksQuery = booksQuery.Where(book => book.Genres.Any(g => g.Name.ToLower().Contains(parameters.GenreName.ToLower())));
        }

        if (parameters.MinPrice is not null)
        {
            booksQuery = booksQuery.Where(book => book.Prices.OrderByDescending(p => p.ValidFrom).First().BookPrice >= parameters.MinPrice);
        }

        if (parameters.MaxPrice is not null)
        {
            booksQuery = booksQuery.Where(book => book.Prices.OrderByDescending(p => p.ValidFrom).First().BookPrice <= parameters.MaxPrice);
        }

        return booksQuery;
    }

    private static IQueryable<Book> ApplyOrderBy(GetBooksModel parameters, IQueryable<Book> booksQuery)
    {
        Expression<Func<Book, string?>> orderFunction = parameters.OrderBy switch
        {
            "name" => book => book.Name,
            "description" => book => book.Description,
            "price" => book => book.Prices.OrderByDescending(p => p.ValidFrom).First().BookPrice
                .ToString(CultureInfo.InvariantCulture),
            "publisher" => book => book.Publisher.Name,
            _ => book => book.Name,
        };

        booksQuery = parameters.OrderDesc == true ? booksQuery.OrderByDescending(orderFunction) : booksQuery.OrderBy(orderFunction);

        return booksQuery;
    }
    
    public Task<BookModel?> GetBookAsync(int bookId)
    {
        return _dbContext.Books
            .Where(b => b.Id == bookId)
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Prices)
            .Select(b => b.MapToBookModel())
            .FirstOrDefaultAsync();
    }
    
    public async Task<BookModel> CreateBookAsync(CreateBookModel model)
    {
        var publisher = await _dbContext.Publishers.FindAsync(model.PublisherId);
        if (publisher == null)
        {
            throw new NotFoundException("Publisher not found.");
        }
        
        var authors = await _dbContext.Authors.Where(a => model.AuthorIds.Contains(a.Id)).ToListAsync();
        if (authors.Count != model.AuthorIds.Count)
        {
            throw new NotFoundException("Author not found.");
        }

        var genres = await _dbContext.Genres.Where(g => model.GenreIds.Contains(g.Id)).ToListAsync();
        if (genres.Count != model.GenreIds.Count)
        {
            throw new NotFoundException("Genre not found.");
        }
        
        if (!model.GenreIds.Contains(model.PrimaryGenreId))
        {
            throw new ArgumentException("PrimaryGenreId must be in GenreIds.");
        }

        var newBook = model.MapToBook();
        newBook.Authors = authors;
        newBook.Genres = genres;
        newBook.PrimaryGenreId = model.PrimaryGenreId;

        var newPrice = new Price
        {
            BookPrice = model.Price,
            ValidFrom = DateTime.Now,
        };
        newBook.Prices.Add(newPrice);

        _dbContext.Books.Add(newBook);
        await SaveAsync();

        return newBook.MapToBookModel();   
    }

    public async Task EditBookAsync(int id, EditBookModel model)
    {
        var book = await _dbContext.Books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book == null)
        {
            throw new NotFoundException("Book not found.");
        }

        if (model.PublisherId is not null)
        {
            var publisher = await _dbContext.Publishers.FindAsync(model.PublisherId);
            if (publisher == null)
            {
                throw new NotFoundException("Publisher not found.");
            }
        }

        book.Isbn = model.Isbn ?? book.Isbn;
        book.Name = model.Name ?? book.Name;
        book.Description = model.Description ?? book.Description;
        book.Count = model.Count ?? book.Count;
        book.PublisherId = model.PublisherId ?? book.PublisherId;

        if (model.AuthorIds is not null)
        {
            if (model.AuthorIds.Count == 0)
            {
                throw new ArgumentException("AuthorIds cannot be empty.");
            }

            var authors = await _dbContext.Authors.Where(a => model.AuthorIds.Contains(a.Id)).ToListAsync();
            if (authors.Count != model.AuthorIds.Count)
            {
                throw new ArgumentException("Author not found.");
            }

            // add those authors that are not in the old list
            foreach (var author in authors.Where(author => !book.Authors.Contains(author)))
            {
                book.Authors.Add(author);
            }

            // remove those authors that are not in the new list
            foreach (var author in book.Authors.ToList().Where(author => !authors.Contains(author)))
            {
                book.Authors.Remove(author);
            }
        }

        if (model.GenreIds is not null || model.PrimaryGenreId is not null)
        {
            if (model.GenreIds is null || model.GenreIds.Count == 0)
            {
                throw new ArgumentException("GenreIds cannot be empty.");
            }
            if (model.PrimaryGenreId is null)
            {
                throw new ArgumentException("PrimaryGenreId cannot be empty.");
            }
            
            if (model.PrimaryGenreId.HasValue && !model.GenreIds.Contains(model.PrimaryGenreId.Value))
            {
                throw new ArgumentException("PrimaryGenreId must be in GenreIds.");
            }

            book.PrimaryGenreId = model.PrimaryGenreId.Value;

            var genres = await _dbContext.Genres.Where(g => model.GenreIds.Contains(g.Id)).ToListAsync();
            if (genres.Count != model.GenreIds.Count)
            {
                throw new NotFoundException("Genre not found.");
            }

            // add those genres that are not in the old list
            foreach (var genre in genres.Where(genre => !book.Genres.Contains(genre)))
            {
                book.Genres.Add(genre);
            }

            // remove those genres that are not in the new list
            foreach (var genre in book.Genres.ToList().Where(genre => !genres.Contains(genre)))
            {
                book.Genres.Remove(genre);
            }
        }

        if (model.Price is not null)
        {
            var newPrice = new Price
            {
                BookPrice = model.Price.Value,
                ValidFrom = DateTime.Now,
            };

            book.Prices.Add(newPrice);
        }

        await SaveAsync();
    }

    public async Task DeleteBookAsync(int bookId)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book == null)
        {
            throw new NotFoundException("Book not found.");
        }

        book.IsDeleted = true;
        
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}