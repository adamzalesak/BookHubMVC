using DataAccessLayer.Models;

namespace BusinessLayer.Models.Order
{
    public class CreateOrderModel
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public long Phone { get; set; }
        public OrderState State { get; set; }
        public decimal? TotalPrice { get; set; }
        public int CartId { get; set; }
        public String UserId { get; set; }
    }
}
