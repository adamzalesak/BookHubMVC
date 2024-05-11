using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models.Publishers;

public class EditPublisherViewModel
{
    [MinLength(1)]
    public string? Name { get; set; }
    
    [MinLength(1)]
    public string? Description { get; set; }
}