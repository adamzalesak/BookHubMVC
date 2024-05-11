namespace WebMVC.Models.Cart;

public class CartViewModel
{
    public int Id { get; set; }
    public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    public decimal TotalPrice { get; set; }
}