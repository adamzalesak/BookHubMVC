using WebMVC.Attributes;

namespace WebMVC.Models.Cart;

public class CartItemViewModel
{
    public int Id;
    public string BookName { get; set; }
    public List<string> Authors { get; set; }
    public decimal BookPrice { get; set; }
    
    [PositiveNumber(ErrorMessage = "Count must be a positive number.")]
    public int CountInCart { get; set; }
    public int CountInStock { get; set; }
    public decimal TotalPrice { get; set; }
}