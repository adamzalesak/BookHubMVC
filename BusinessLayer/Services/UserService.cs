using BusinessLayer.Mappers;
using BusinessLayer.Models.User;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly BookHubDbContext _dbContext;

    public UserService(BookHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserModel> CreateUserAsync(CreateUserModel model)
    {
        var newUser = model.MapToUser();

        await _dbContext.AppUsers.AddAsync(newUser);
        await SaveAsync();

        return newUser.MapToUserModel();
    }

    public async Task<UserModel?> EditUserAsync(String userId, EditUserModel model)
    {
        var user = await _dbContext.AppUsers.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        user.Name = model.Name ?? user.Name;
        user.UserName = model.UserName ?? user.UserName;
        user.Email = model.Email ?? user.Email;
        user.IsAdministrator = model.IsAdministrator ?? user.IsAdministrator;
        user.CartId = model.CartId ?? user.CartId;

        await SaveAsync();
        
        return user.MapToUserModel();
    }

    public async Task<UserModel?> GetUserByIdAsync(String userId)
    {
        var userModel = await _dbContext.AppUsers
            .Where(u => u.Id == userId)
            .Include(u => u.Orders)
            .Select(u => u.MapToUserModel())
            .FirstOrDefaultAsync();

        return userModel;
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
        var userModels = await _dbContext.AppUsers
            .Include(u => u.Orders)
            .Select(u => u.MapToUserModel())
            .ToListAsync();
        
        return userModels;
    }

    public async Task<bool> DeleteUserAsync(String userId)
    {
        var user = await _dbContext.AppUsers.FindAsync(userId);
        if (user == null)
        {
            return false;
        }
        
        _dbContext.AppUsers.Remove(user);
        await SaveAsync();

        return true;
    }
    
    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}