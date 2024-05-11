using WebMVC.Views.Orders;

namespace WebMVC.Models.Orders;

public class EditOrdersViewModel
{
    public required ICollection<OrderViewModel> Orders { get; set; }
}