using BusinessLayer.Models.User;

namespace BusinessLayer.Services.Abstraction;

public interface IUserService : IBaseService
{
    public Task<UserModel> CreateUserAsync(CreateUserModel model);
    public Task<UserModel?> EditUserAsync(String userId, EditUserModel model); 
    public Task<UserModel?> GetUserByIdAsync(String  userId);
    public Task<List<UserModel>> GetUsersAsync();
    public Task<bool> DeleteUserAsync(String userId);
}