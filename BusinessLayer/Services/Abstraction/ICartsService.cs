using BusinessLayer.Models.Cart;
using DataAccessLayer.Models;

namespace BusinessLayer.Services.Abstraction
{
    public interface ICartsService : IBaseService
    {
        public Task<CartModel?> GetCart(int id);
        public Task<List<CartModel>> GetAllCarts();
        public Task<CartModel?> CreateCart(CreateCartModel createCartModel);
        public Task<bool> DeleteCart(int id);
        public Task<CartItemModel> GetCartItem(int cartItemId);
        public Task AddBookToCart(int cartId, int bookId);
        public Task ChangeCartItemCount(int cartItemId, int newCount);
        public Task RemoveCartItem(int cartItemId);
    }
}
