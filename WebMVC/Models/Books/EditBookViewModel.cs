using System.ComponentModel.DataAnnotations;
using WebMVC.Attributes;

namespace WebMVC.Models;

public class EditBookViewModel
{
    public string? Isbn { get; set; }
    
    [MinLength(1)]
    public string? Name { get; set; }
    
    [MinLength(1)]
    public string? Description { get; set; }
    
    [NonNegativeNumber(ErrorMessage = "Count must be a non-negative number.")]
    public int? Count { get; set; }
    
    [NonNegativeNumber(ErrorMessage = "Price must be a non-negative number.")]
    public decimal? Price { get; set; }
}