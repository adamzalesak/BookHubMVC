using BusinessLayer.Models.Genre;
using BusinessLayer.Models.Publisher;

namespace WebMVC.Models;

public class SidebarViewModel
{
    public required ICollection<GenreModel> Genres { get; set; }
    public required ICollection<PublisherModel> Publishers { get; set; }
}