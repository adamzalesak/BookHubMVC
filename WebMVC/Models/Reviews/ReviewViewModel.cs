namespace WebMVC.Models.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public int BookId { get; set; }
        public string Username { get; set; }
    }
}
