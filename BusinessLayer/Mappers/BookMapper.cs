using BusinessLayer.Models.Book;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers;

[Mapper]
public static partial class BookMapper
{
    public static partial Book MapToBook(this CreateBookModel model);
    [MapProperty(nameof(Book.Prices), nameof(BookModel.Price))]
    public static partial BookModel MapToBookModel(this Book book);
    public static partial List<BookModel> MapToBookModelList(this ICollection<Book> book);

    private static string AuthorToAuthor(Author author) => author.Name;
    private static string PublisherToPublisher(Publisher publisher) => publisher.Name;
    private static string GenreToGenre(Genre genre) => genre.Name;
    private static decimal PricesToPrice(ICollection<Price> prices)
    {
        return prices.OrderByDescending(p => p.ValidFrom).First().BookPrice;
    }
}
