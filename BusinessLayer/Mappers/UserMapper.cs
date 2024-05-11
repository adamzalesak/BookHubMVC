using BusinessLayer.Models.User;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers;

[Mapper]
public static partial class UserMapper
{
    public static partial User MapToUser(this CreateUserModel model);
    
    [MapProperty(nameof(User.Orders), nameof(UserModel.OrderIds))]
    public static partial UserModel MapToUserModel(this User user);

    private static List<int> OrdersToOrderIds(ICollection<Order> orders)
    {
        return orders.Select(o => o.Id).ToList();
    }
}