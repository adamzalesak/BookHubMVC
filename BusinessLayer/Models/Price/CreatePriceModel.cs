namespace BusinessLayer.Models.Price
{
    public class CreatePriceModel
    {
        public decimal BookPrice { get; set; }
        public DateTime ValidFrom { get; set; }
        public int BookId { get; set; }
    }
}
