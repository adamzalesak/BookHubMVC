using BusinessLayer.Models.Cart;
using BusinessLayer.Models.Order;

namespace BusinessLayer.Facades;

public interface IOrderFacade
{
    public Task<DetailedCartModel> GetDetailedCart(int cartId);
    public Task AddBookToCart(int cartId, int bookId);
    public Task ChangeCartItemCount(int cartItemId, int newCount);
    public Task RemoveCartItem(int cartItemId);
    public Task CreateOrder(CreateOrderModel model);
}