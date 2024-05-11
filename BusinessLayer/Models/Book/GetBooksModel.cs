namespace BusinessLayer.Models.Book;

public class GetBooksModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public int? PublisherId { get; set; }
    public string? PublisherName { get; set; }
    public int? GenreId { get; set; }
    public string? GenreName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? OrderBy { get; set; }
    public bool? OrderDesc { get; set; }
}
