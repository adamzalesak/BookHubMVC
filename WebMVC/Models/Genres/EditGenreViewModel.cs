using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models;

public class EditGenreViewModel
{
    [MinLength(1)]
    public string Name { get; set; }
}