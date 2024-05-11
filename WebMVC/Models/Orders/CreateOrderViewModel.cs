using System.ComponentModel.DataAnnotations;
using WebMVC.Attributes;

namespace WebMVC.Models.Orders;

public class CreateOrderViewModel
{
    [EmailAddress]
    public string Email { get; set; }
    
    [MinLength(1)]
    public string Address { get; set; }

    [NonNegativeNumber(ErrorMessage = "Invalid phone number.")]
    public long Phone { get; set; }
}