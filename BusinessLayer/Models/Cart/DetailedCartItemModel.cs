namespace BusinessLayer.Models.Cart;

public class DetailedCartItemModel
{
    public int Id;
    public string BookName { get; set; }
    public List<string> Authors { get; set; }
    public decimal BookPrice { get; set; }
    public int CountInCart { get; set; }
    public int CountInStock { get; set; }
    public decimal TotalPrice { get; set; }
}