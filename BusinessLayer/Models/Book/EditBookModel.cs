namespace BusinessLayer.Models.Book;

public class EditBookModel
{
    public string? Isbn { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Count { get; set; }
    public decimal? Price { get; set; }
    public ICollection<int>? AuthorIds { get; set; }
    public ICollection<int>? GenreIds { get; set; }
    public int? PrimaryGenreId { get; set; }
    public int? PublisherId { get; set; }
}