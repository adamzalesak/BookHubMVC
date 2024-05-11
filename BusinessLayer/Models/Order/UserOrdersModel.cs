using BusinessLayer.Models.Book;
using DataAccessLayer.Models;

namespace BusinessLayer.Models.Order
{
    public class UserOrdersModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderState State { get; set; }
        public DateTime Timestamp { get; set; }
        public List<String> Books { get; set; }
    }
}
