namespace BusinessLayer.Models.Book;

public class BookPaginationModel
{
    public ICollection<BookModel> Books { get; set; }
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
}