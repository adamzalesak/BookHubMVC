namespace DataAccessLayer.Models;

public class Review : BaseEntity
{
    public int Rating { get; set; }
    public string Text { get; set; }
    public String UserId { get; set; }
    public User User { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}