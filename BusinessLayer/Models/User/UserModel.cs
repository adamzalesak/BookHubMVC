namespace BusinessLayer.Models.User;

public class UserModel
{
    public String Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<int> OrderIds { get; set; }
}