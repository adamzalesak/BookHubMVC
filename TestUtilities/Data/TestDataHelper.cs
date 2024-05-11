using DataAccessLayer.Models;

namespace TestUtilities.Data;

public static class TestDataHelper
{
    public static List<Author> GetFakeAuthors()
    {
        return new List<Author>()
        {
            new Author
            {
                Id = 1,
                Name = "Joanne Rowling",
            },
            new Author
            {
                Id = 2,
                Name = "Jo Nesbo",
            }
        };
    }
    
    public static List<Publisher> GetFakePublishers()
    {
        return new List<Publisher>()
        {
            new Publisher
            {
                Id = 1,
                Name = "Albatros",
                Description = "Albatros",
            },
            new Publisher
            {
                Id = 2,
                Name = "Dobrovský",
                Description = "Dobrovský",
            },
            new Publisher
            {
                Id = 3,
                Name = "Argus",
                Description = "Argus",
            }
        };
    }

    public static List<Genre> GetFakeGenres()
    {
        return new List<Genre>()
        {
            new Genre
            {
                Id = 1,
                Name = "Fantasy"
            },
            new Genre
            {
                Id = 2,
                Name = "Detektivka"
            },
        };
    }
    
    public static List<Price> GetFakePrices()
    {
        return new List<Price>()
        {
            new Price
            {
                Id = 1,
                BookId = 1,
                BookPrice = 349.99M,
                ValidFrom = DateTime.UtcNow.AddDays(-10)
            },
            new Price
            {
                Id = 2,
                BookId = 1,
                BookPrice = 299.99M,
                ValidFrom = DateTime.UtcNow.AddDays(-5)
            },
            new Price
            {
                Id = 3,
                BookId = 2,
                BookPrice = 249.99M,
                ValidFrom = DateTime.UtcNow.AddDays(-20)
            },
            new Price
            {
                Id = 4,
                BookId = 3,
                BookPrice = 199.99M,
                ValidFrom = DateTime.UtcNow.AddDays(-30)
            },
            new Price
            {
                Id = 5,
                BookId = 4,
                BookPrice = 499.00M,
                ValidFrom = DateTime.UtcNow.AddDays(-40)
            },
            new Price
            {
                Id = 6,
                BookId = 5,
                BookPrice = 445.50M,
                ValidFrom = DateTime.UtcNow.AddDays(-45)
            },
        };
    }
    
    public static List<Book> GetFakeBooks()
    {
        return new List<Book>()
        {
            new Book
            {
                Id = 1,
                Name = "Harry Potter a Kámen mudrců",
                Description = "Harry Potter a Kámen mudrců",
                Isbn = "12345",
                PublisherId = 1,
                PrimaryGenreId = 1,
            },
            new Book
            {
                Id = 2,
                Name = "Harry Potter a vězeň z Azkabanu",
                Description = "Harry Potter a vězeň z Azkabanu",
                Isbn = "12345",
                PublisherId = 2,
                PrimaryGenreId = 1,
            },
            new Book
            {
                Id = 3,
                Name = "Harry Potter a Tajemná komnata",
                Description = "Harry Potter a Tajemná komnata",
                Isbn = "12345",
                PublisherId = 1,
                PrimaryGenreId = 1,
            },
            new Book
            {
                Id = 4,
                Name = "Zatmění",
                Description = "Dobrodružství Harryho Hola",
                Isbn = "12345",
                PublisherId = 2,
                PrimaryGenreId = 1,
            },
            new Book
            {
                Id = 5,
                Name = "Krev na sněhu",
                Description = "Dobrodružství Harryho Hola",
                Isbn = "12345",
                PublisherId = 3,
                PrimaryGenreId = 1,
            }
        };
    }

    public static List<AuthorBook> GetFakeAuthorBooks()
    {
        return new List<AuthorBook>()
        {
            new AuthorBook
            {
                AuthorId = 1,
                BookId = 1,
            },
            new AuthorBook
            {
                AuthorId = 1,
                BookId = 2,
            },
            new AuthorBook
            {
                AuthorId = 1,
                BookId = 3,
            },
            new AuthorBook
            {
                AuthorId = 2,
                BookId = 4,
            },
            new AuthorBook
            {
                AuthorId = 2,
                BookId = 5,
            },
        };
    }

    public static List<BookGenre> GetFakeBookGenres()
    {
        return new List<BookGenre>()
        {
            new BookGenre
            {
                BookId = 1,
                GenreId = 1,
            },
            new BookGenre
            {
                BookId = 2,
                GenreId = 1,
            },
            new BookGenre
            {
                BookId = 3,
                GenreId = 1,
            },
            new BookGenre
            {
                BookId = 4,
                GenreId = 2,
            },
            new BookGenre
            {
                BookId = 5,
                GenreId = 2,
            },
        };
    }
    
    public static List<User> GetFakeUsers()
    {
        return new List<User>
        {
            new()
            {
                Id = "1",
                Name = "John Doe",
                Email = "john.doe@email.com",
                UserName = "johndoe",
            }
        };
    }
    
    public static List<Cart> GetFakeCarts()
    {
        return new List<Cart>
        {
            new()
            {
                Id = 1,
            },
            new()
            {
                Id = 2,
            },
        };
    }

    public static List<Order> GetFakeOrders()
    {
        return new List<Order>
        {
            new()
            {
                Id = 1,
                UserId = "1",
                CartId = 1,
                Address = "Test Address",
                Email = "test@email.com",
                Phone = 420605123456,
                TotalPrice = 123.45m,
            },
        };
    }
}