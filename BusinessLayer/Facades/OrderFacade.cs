using BusinessLayer.Exceptions;
using BusinessLayer.Models.Book;
using BusinessLayer.Models.Cart;
using BusinessLayer.Models.Order;
using BusinessLayer.Models.User;
using BusinessLayer.Services.Abstraction;

namespace BusinessLayer.Facades;

public class OrderFacade : IOrderFacade
{
    private readonly ICartsService _cartsService;
    private readonly IBooksService _booksService;
    private readonly IOrdersService _ordersService;
    private readonly IUserService _userService;
    
    public OrderFacade(ICartsService cartsService, IBooksService booksService, IOrdersService ordersService, IUserService userService)
    {
        _booksService = booksService;
        _cartsService = cartsService;
        _ordersService = ordersService;
        _userService = userService;
    }

    public async Task<DetailedCartModel> GetDetailedCart(int cartId) 
    {
        var cart = await _cartsService.GetCart(cartId);
        if (cart == null)
        {
            throw new NotFoundException($"Cart not found.");
        }

        var detailedCart = new DetailedCartModel()
        {
            Id = cart.Id,
            CartItems = new List<DetailedCartItemModel>(),
            TotalPrice = 0
        };

        foreach (var cartItem in cart.CartItems)
        {
            var book = await _booksService.GetBookAsync(cartItem.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book not found.");
            }

            var detailedCartItem = new DetailedCartItemModel()
            {
                Id = cartItem.Id,
                Authors = book.Authors,
                BookName = book.Name,
                BookPrice = book.Price,
                CountInCart = cartItem.Count,
                CountInStock = book.Count,
                TotalPrice = cartItem.Count * book.Price
            };
            detailedCart.CartItems.Add(detailedCartItem);
            detailedCart.TotalPrice += detailedCartItem.TotalPrice;
        }
        
        return detailedCart;
    }

    public async Task AddBookToCart(int cartId, int bookId)
    {
        var book = await _booksService.GetBookAsync(bookId);
        if (book == null)
        {
            throw new NotFoundException($"Book not found.");
        }
        if (book.IsDeleted)
        {
            throw new InvalidOperationException($"Book is deleted.");
        }

        if (book.Count == 0)
        {
            throw new InvalidOperationException($"Book is out of stock, cannot be added to cart.");
        }

        await _cartsService.AddBookToCart(cartId, bookId);

        // reserve the book for the user
        var editBookModel = new EditBookModel()
        {
            Count = book.Count - 1
        };
        await _booksService.EditBookAsync(bookId, editBookModel);
    }

    public async Task ChangeCartItemCount(int cartItemId, int newCount)
    {
        if (newCount < 1)
        {
            throw new ArgumentException("New book count cannot be negative or zero.");
        }

        var cartItem = await _cartsService.GetCartItem(cartItemId);
        
        // optimization
        if (newCount == cartItem.Count)
        {
            return;
        }
        
        var book = await _booksService.GetBookAsync(cartItem.BookId);
        if (book == null)
        {
            throw new NotFoundException($"Book not found.");
        }

        if (newCount > cartItem.Count && newCount - cartItem.Count > book.Count)
        {
            throw new InvalidOperationException($"Changing count from {cartItem.Count} to {newCount} would cause the book to get out of stock.");
        }

        await _cartsService.ChangeCartItemCount(cartItemId, newCount);

        // reserve the books for the user or increase the book count if newCount is smaller than bookInCartCount
        var editBookModel = new EditBookModel()
        {
            Count = book.Count - (newCount - cartItem.Count)
        };
        await _booksService.EditBookAsync(cartItem.BookId, editBookModel);
    }

    public async Task RemoveCartItem(int cartItemId)
    {
        var cartItem = await _cartsService.GetCartItem(cartItemId);
        
        var book = await _booksService.GetBookAsync(cartItem.BookId);
        if (book == null)
        {
            throw new NotFoundException($"Book not found.");
        }
        
        await _cartsService.RemoveCartItem(cartItemId);

        var editBookModel = new EditBookModel()
        {
            Count = book.Count + cartItem.Count
        };
        await _booksService.EditBookAsync(cartItem.BookId, editBookModel);
    }

    public async Task CreateOrder(CreateOrderModel model)
    {
        var cart = await _cartsService.GetCart(model.CartId);
        if (cart == null)
        {
            throw new NotFoundException("Cart not found");
        }

        var totalPrice = await GetCartTotalPrice(cart); 
        model.TotalPrice = totalPrice;
        
        var newOrder = await _ordersService.CreateOrder(model);
        if (newOrder == null)
        {
            throw new CreationFailedException("Failed to create new order");
        }

        var user = await _userService.GetUserByIdAsync(model.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User not found.");
        }
        
        // create new empty Cart for the user
        var newCart = await _cartsService.CreateCart(new CreateCartModel());
        if (newCart == null)
        {
            throw new CreationFailedException("Failed to create new cart for the user");
        }
        
        var editUserModel = new EditUserModel()
        {
            CartId = newCart.Id
        };
        var editResult = await _userService.EditUserAsync(model.UserId, editUserModel);
        if (editResult == null)
        {
            throw new EditingFailedException("Failed to update users cart");
        }
    }

    private async Task<decimal> GetCartTotalPrice(CartModel cart)
    {
        decimal totalPrice = 0;
        foreach (var cartItem in cart.CartItems)
        {
            var book = await _booksService.GetBookAsync(cartItem.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book not found.");
            }

            totalPrice += cartItem.Count * book.Price;
        }

        return totalPrice;
    }
}