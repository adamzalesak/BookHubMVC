using System.Globalization;
using BusinessLayer.Exceptions;
using BusinessLayer.Facades;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Mappers;
using WebMVC.Models.Cart;

namespace WebMVC.Controllers;

[Route("cart")]
public class CartController : Controller
{
    private readonly IOrderFacade _orderFacade;
    private readonly UserManager<User> _userManager;
    
    public CartController(IOrderFacade orderFacade, UserManager<User> userManager)
    {
        _orderFacade = orderFacade;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        var user = await GetLoggedInUser();
        if (user == null)
        {
            return BadRequest("Cannot show cart, no user is logged in.");
        }

        try
        {
            var detailedCart = await _orderFacade.GetDetailedCart(user.CartId);
            return View(detailedCart.MapToCartViewModel());
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = "An error occured while loading cart";
            return View("ErrorMessage");
        }
    }

    [HttpPost("add-book/{bookId:int}")]
    public async Task<IActionResult> AddBookToCart(int bookId)
    {
        var user = await GetLoggedInUser();
        if (user == null)
        {
            return BadRequest("Cannot add book to cart, no user is logged in.");
        }

        try
        {
            await _orderFacade.AddBookToCart(user.CartId, bookId);
        }
        catch (BookAlreadyInCartException e)
        {
            return RedirectToAction("Cart", new { cartId = user.CartId });
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = "An error occured while adding book to cart";
            return View("ErrorMessage");
        }

        return RedirectToAction("Cart", new { cartId = user.CartId });
    }

    [HttpPost("change-item-count/{cartItemId:int}")]
    public async Task<IActionResult> ChangeCartItemCount(int cartItemId, CartItemViewModel model)
    {
        var user = await GetLoggedInUser();
        if (user == null)
        {
            return BadRequest("Cannot change cart item count, no user is logged in.");
        }

        try
        {
            await _orderFacade.ChangeCartItemCount(cartItemId, model.CountInCart);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = "An error occured while changing cart item count";
            return View("ErrorMessage");
        }
        
        return RedirectToAction("Cart", new { cartId = user.CartId });
    }

    [HttpPost("remove-item/{cartItemId:int}")]
    public async Task<IActionResult> RemoveCartItem(int cartItemId)
    {
        var user = await GetLoggedInUser();
        if (user == null)
        {
            return BadRequest("Cannot remove cart item, no user is logged in.");
        }
        
        try
        {
            await _orderFacade.RemoveCartItem(cartItemId);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = "An error occured while removing item from cart";
            return View("ErrorMessage");
        }

        return RedirectToAction("Cart", new { cartId = user.CartId });
    }

    private async Task<User?> GetLoggedInUser()
    {
        var username = User.Identity?.Name;
        if (username == null)
        {
            return null;
        }
        
        return await _userManager.FindByNameAsync(username);
    }
}