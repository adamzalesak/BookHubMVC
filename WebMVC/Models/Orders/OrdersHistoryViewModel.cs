using DataAccessLayer.Models;

namespace WebMVC.Models.Orders
{
    public class OrdersHistoryViewModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderState State { get; set; }
        public DateTime Timestamp { get; set; }
        public List<String> Books { get; set; }
    }
}
