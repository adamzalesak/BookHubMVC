namespace BusinessLayer.Models.Review;

public class ReviewModel
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; }
    public int BookId { get; set; }
    public string Username { get; set; }
}