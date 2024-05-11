namespace DataAccessLayer.Models;

public class Cart : BaseEntity
{
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public Order? Order { get; set; }
}