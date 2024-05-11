namespace BusinessLayer.Models.User;

public class EditUserModel
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool? IsAdministrator { get; set; }
    public int? CartId { get; set; }
}