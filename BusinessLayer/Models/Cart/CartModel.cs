using DataAccessLayer.Models;

namespace BusinessLayer.Models.Cart;

public class CartModel
{
    public int Id { get; set; }
    public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    public int? OrderId { get; set; }
}