using WebMVC.Models.Review;

namespace WebMVC.Models.Books
{
    public class CombinedBookReviewViewModel
    {
        public BookViewModel? BookViewModel { get; set; }
        public CreateReviewViewModel? CreateReviewViewModel { get; set; }
    }
}
