using DataAccessLayer.Models;
using WebMVC.Models.Review;

namespace WebMVC.Models;

public class BookViewModel
{
    public int Id { get; set; }
    public string Isbn { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Genres { get; set; }
    public string PrimaryGenre { get; set; }
    public string Publisher { get; set; }
    public List<ReviewViewModel> Reviews { get; set; }
}