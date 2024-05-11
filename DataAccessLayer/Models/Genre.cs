namespace DataAccessLayer.Models;

public class Genre : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}