using System.ComponentModel.DataAnnotations;
using WebMVC.Attributes;

namespace WebMVC.Models.Review
{
    public class CreateReviewViewModel
    {
        [Required]
        public int? BookId { get; set; }
        [Required]
        public String? UserId { get; set; }
        [Required]
        [Range(0, 5)]
        public int? Rating { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Minimal text length is 10 characters")]
        public string? Text { get; set; }
    }
}
