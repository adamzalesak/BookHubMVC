namespace BusinessLayer.Models.Cart
{
    public class CreateCartModel
    {
        public List<int> CartItemIds { get; set; } = new List<int>();
        public int? OrderId { get; set; }
    }
}
