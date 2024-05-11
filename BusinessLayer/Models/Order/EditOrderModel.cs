using DataAccessLayer.Models;

namespace BusinessLayer.Models.Order;

public class EditOrderModel
{
    public string? Email { get; set; }
    public string? Address { get; set; }
    public long? Phone { get; set; }
    public OrderState? State { get; set; }
}