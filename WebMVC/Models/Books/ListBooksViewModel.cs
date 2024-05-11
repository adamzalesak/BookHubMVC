using BusinessLayer.Models.Genre;
using BusinessLayer.Models.Publisher;

namespace WebMVC.Models.Books;

public class ListBooksViewModel
{
    public required ICollection<BookViewModel> Books { get; set; }
    public string? FilteredGenreName { get; set; }
    public string? FilteredPublisherName { get; set; }
    public ICollection<GenreModel>? FoundGenres { get; set; }
    public ICollection<PublisherModel>? FoundPublishers { get; set; }
    public int PageCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int? GenreId { get; set; }
    public int? PublisherId { get; set; }
    public String? SearchString { get; set; }
}