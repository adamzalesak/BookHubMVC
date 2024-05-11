namespace DataAccessLayer.Models;

public class Book : BaseEntity
{
    public string Isbn { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Count { get; set; }
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public int PrimaryGenreId { get; set; }
    public Genre PrimaryGenre { get; set; }
    public ICollection<Price> Prices { get; set; } = new List<Price>();
    public bool IsDeleted { get; set; }
}