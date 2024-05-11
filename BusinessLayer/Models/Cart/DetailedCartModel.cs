namespace BusinessLayer.Models.Cart;

public class DetailedCartModel
{
    public int Id { get; set; }
    public List<DetailedCartItemModel> CartItems { get; set; } = new List<DetailedCartItemModel>();
    public decimal TotalPrice { get; set; }
    public int? OrderId { get; set; }
}