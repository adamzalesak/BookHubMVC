using DataAccessLayer.Models;

namespace WebMVC.Views.Orders;

public class OrderViewModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public long Phone { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderState State { get; set; }
    public DateTime Timestamp { get; set; }
}