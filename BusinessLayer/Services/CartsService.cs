using BusinessLayer.Exceptions;
using BusinessLayer.Mappers;
using BusinessLayer.Models.Cart;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class CartsService : ICartsService
    {
        private readonly BookHubDbContext _dbContext;

        public CartsService(BookHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<CartModel?> GetCart(int id)
        {
            var cart = await GetCartObject(id);
            if (cart == null)
            {
                return null;
            }

            return cart.MapToCartModel();
        }
        
        public async Task<List<CartModel>> GetAllCarts()
        {
            var carts = await _dbContext.Carts
                .Include(c => c.CartItems)
                .Include(c => c.Order)
                .ToListAsync();
            return carts.MapToCartModelList();
        }

        public async Task<CartModel?> CreateCart(CreateCartModel createCartModel)
        {
            var newCart = new Cart
            {
                CartItems = await _dbContext.CartItems.Where(ci => createCartModel.CartItemIds.Contains(ci.Id)).ToListAsync(),
                Order = await _dbContext.Orders.SingleOrDefaultAsync(o => o.Id == createCartModel.OrderId),
            };
            var cart = await _dbContext.Carts.AddAsync(newCart);
            await SaveAsync();
            return cart.Entity.MapToCartModel();
        }

        public async Task<bool> DeleteCart(int id)
        {
            var cart = await GetCartObject(id);
            if (cart == null)
            {
                throw new NotFoundException($"Cart not found");
            }
            _dbContext.Carts.Remove(cart);
            await SaveAsync();
            return true;
        }

        public async Task<CartItemModel> GetCartItem(int cartItemId)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                throw new NotFoundException($"Cart item not found.");
            }

            return cartItem.MapToCartItemModel();
        }
        
        public async Task AddBookToCart(int cartId, int bookId)
        {
            // check to avoid putting inconsistent data into database
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                throw new NotFoundException($"Book not found.");
            }
            if (book.IsDeleted)
            {
                throw new InvalidOperationException($"Book is deleted.");
            }
            
            var cart = await GetCartObject(cartId);
            if (cart == null)
            {
                throw new NotFoundException($"Cart not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
            
            // if cartItem with given bookId already exists
            if (cartItem != null)
            {
                throw new BookAlreadyInCartException("Book is already in cart.");
            }

            // else create new cart item
            var newCartItem = new CartItem()
            {
                CartId = cartId,
                BookId = bookId,
                Count = 1
            };

            _dbContext.CartItems.Add(newCartItem);
            await SaveAsync();
        }

        public async Task ChangeCartItemCount(int cartItemId, int newCount)
        {
            if (newCount < 1)
            {
                throw new ArgumentException("New cart item count cannot be negative or zero.");
            }
            
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                throw new NotFoundException($"Cart item not found.");
            }

            cartItem.Count = newCount;
            await SaveAsync();
        }

        public async Task RemoveCartItem(int cartItemId)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                throw new NotFoundException($"Cart item not found.");
            }
            
            _dbContext.CartItems.Remove(cartItem);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Cart?> GetCartObject(int id)
        {
            return await _dbContext.Carts
                .Where(c => c.Id == id)
                .Include(c => c.CartItems)
                .Include(c => c.Order)
                .FirstOrDefaultAsync();
        }
    }
}
