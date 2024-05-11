namespace BusinessLayer.Models.Cart;

public class CartItemModel
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int BookId { get; set; }
    public int Count { get; set; }
}