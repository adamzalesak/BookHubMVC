using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Models;
using WebMVC.Attributes;

namespace WebMVC.Models;

public class EditOrderViewModel
{
    [EmailAddress]
    public string Email { get; set; }
    
    [MinLength(1)]
    public string Address { get; set; }
    
    [NonNegativeNumber(ErrorMessage = "Invalid phone number.")]
    public long Phone { get; set; }
    
    public OrderState State { get; set; }
}