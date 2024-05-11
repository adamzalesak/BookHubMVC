using BusinessLayer.Exceptions;
using BusinessLayer.Models.Book;
using BusinessLayer.Services;
using DataAccessLayer.Data;
using rgvlee.Core.Common.Extensions;
using TestUtilities.MockedObjects;

namespace BusinessLayer.Tests.Services;

public class BooksServiceTests
{
    private readonly BookHubDbContext _dbContext;
    private readonly BooksService _booksService;

    public BooksServiceTests()
    {
        var dbContextOptions = MockedDbContext.GenerateNewInMemoryDbContextOptions();
        _dbContext = MockedDbContext.CreateFromOptions(dbContextOptions);
        _booksService = new BooksService(_dbContext);
    }
    
    [Fact]
    public async Task GetBooksAsync_FilterByName_ExactMatch()
    {
        // arrange
        var parameters = new GetBooksModel { Name = "harry potter" };
        var bookIds = await _dbContext.Books
            .Where(b => b.Name.ToLower().Contains(parameters.Name))
            .Select(b => b.Id)
            .ToListAsync();
        
        // act and assert
        await ActAndAssertGetBooksAsync(bookIds, parameters); }
    
    [Fact]
    public async Task GetBooksAsync_FilterByAuthorName_ExactMatch()
    {
        // arrange
        var parameters = new GetBooksModel { AuthorName = "jo nesbo" };
        var bookIds = await _dbContext.Books
            .Where(b => b.Authors.Any(a => a.Name.ToLower().Contains(parameters.AuthorName.ToLower())))
            .Select(b => b.Id)
            .ToListAsync();
        
        // act and assert
        await ActAndAssertGetBooksAsync(bookIds, parameters);
    }
    
    [Fact]
    public async Task GetBooksAsync_FilterByPublisherId_ExactMatch()
    {
        // arrange
        var parameters = new GetBooksModel { PublisherId = 2 };
        var bookIds = await _dbContext.Books
            .Where(b => b.PublisherId == parameters.PublisherId)
            .Select(b => b.Id)
            .ToListAsync();
        
        // act and assert
        await ActAndAssertGetBooksAsync(bookIds, parameters);
    }
    
    [Fact]
    public async Task GetBooksAsync_FilterByGenreId_ExactMatch()
    {
        // arrange
        var parameters = new GetBooksModel { GenreId = 1 };
        var bookIds = await _dbContext.Books
            .Where(b => b.Genres.Any(g => g.Id == parameters.GenreId))
            .Select(b => b.Id)
            .ToListAsync();
        
        // act and assert
        await ActAndAssertGetBooksAsync(bookIds, parameters);
    }

    [Fact]
    public async Task GetBooksAsync_FilterByMinMaxPrice_PricesAreInRange()
    {
        var book = _dbContext.Books.ToList();
        
        // arrange
        var parameters = new GetBooksModel
        {
            MinPrice = 200.00M,
            MaxPrice = 300.00M,
        };
        
        // act
        var result = await _booksService.GetBooksAsync(parameters);
        
        // assert
        Assert.NotNull(result);
        Assert.All(result.Books, bookModel => 
            Assert.True(bookModel.Price >= parameters.MinPrice && bookModel.Price <= parameters.MaxPrice)
        );
    }

    [Fact]
    public async Task GetBooksAsync_Paging_ExactMatch()
    {
        // arrange
        const int page = 1; // 0-based page indexing
        const int pageSize = 3;
        var parameters = new GetBooksModel
        {
            Page = page,
            PageSize = pageSize,
        };
        var bookIds = await _dbContext.Books
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(b => b.Id)
            .ToListAsync();
        
        // act and assert
        await ActAndAssertGetBooksAsync(bookIds, parameters);
    }
    
    [Fact]
    public async Task GetBooksAsync_OrderByPriceAscending_ExactMatch()
    {
        // arrange
        var parameters = new GetBooksModel
        {
            OrderBy = "price",
            OrderDesc = false,
        };
        var bookIds = await _dbContext.Books
            .OrderBy(b => b.Prices.OrderByDescending(p => p.ValidFrom).First().BookPrice)
            .Select(b => b.Id)
            .ToListAsync();
        
        // act and assert
        await ActAndAssertGetBooksAsync(bookIds, parameters);
    }

    private async Task ActAndAssertGetBooksAsync(List<int> bookIds, GetBooksModel parameters)
    {
        // act
        var result = await _booksService.GetBooksAsync(parameters);
        
        // assert
        Assert.NotNull(result);
        Assert.Equal(bookIds.Count, result.Books.Count);
        Assert.All(result.Books, bookModel => Assert.Contains(bookModel.Id, bookIds));
    }
    
    [Fact]
    public async Task CreateBookAsync_ValidInput_ReturnsBookModel()
    {
        // arrange
        var model = new CreateBookModel
        {
            Name = "Kniha 1",
            Description = "Kniha 1",
            Isbn = "12345",
            Count = 5,
            Price = 159.99M,
            AuthorIds = new List<int>() { 1, 2 },
            GenreIds = new List<int>() { 1, 2 },
            PrimaryGenreId = 1,
            PublisherId = 1,
        };

        // act
        var result = await _booksService.CreateBookAsync(model);

        // assert
        Assert.NotNull(result);
        Assert.Equal(model.Name, result.Name);
        Assert.Equal(model.Description, result.Description);
        Assert.Equal(model.Isbn, result.Isbn);
        Assert.Equal(model.Count, result.Count);
        Assert.True(result.Authors.Count == 2);
        Assert.True(result.Genres.Count == 2);
    }
    
    [Fact]
    public async Task CreateBookAsync_InvalidInput_ThrowsException()
    {
        // arrange
        var model = new CreateBookModel
        {
            Name = "Kniha 1",
        };

        // act and assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _booksService.CreateBookAsync(model));
    }
    
    [Fact]
    public async Task EditBookAsync_EditsBook()
    {
        // arrange
        const int bookId = 1;
        var updateModel = new EditBookModel
        {
            Name = "Harry Potter a jeho svetr",
            Description = "Harry Potter a jeho svetr",
        };

        // act
        await _booksService.EditBookAsync(bookId, updateModel);
        var editedBook = await _booksService.GetBookAsync(bookId);

        // assert
        Assert.NotNull(editedBook);
        Assert.Equal(updateModel.Name, editedBook.Name);
        Assert.Equal(updateModel.Description, editedBook.Description);
    }
    
    [Fact]
    public async Task DeleteBookAsync_ExistingBook_SetsIsDeleted()
    {
        // arrange
        var bookId = 1;
        var book = await _dbContext.Books
            .Where(b => b.Id == bookId)
            .FirstOrDefaultAsync();

        // act
        await _booksService.DeleteBookAsync(bookId);

        // assert
        Assert.True(book == null || book.IsDeleted);
    }

    [Fact]
    public async Task DeleteBookAsync_NonExistingBook_ThrowsException()
    {
        // arrange
        var bookId = 100;

        // act and assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _booksService.DeleteBookAsync(bookId));
    }
}