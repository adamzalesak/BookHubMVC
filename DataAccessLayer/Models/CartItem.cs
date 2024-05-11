namespace DataAccessLayer.Models;

public class CartItem : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; }
    public int Count { get; set; }
}