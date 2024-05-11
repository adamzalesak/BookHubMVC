using BusinessLayer.Facades;
using BusinessLayer.Models.Order;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Mappers;
using WebMVC.Models;
using WebMVC.Models.Orders;

namespace WebMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderFacade _orderFacade;
        private readonly IOrdersService _orderService;
        private readonly UserManager<User> _userManager;

        public OrdersController(IOrdersService orderService, UserManager<User> userManager, IOrderFacade orderFacade)
        {
            _orderService = orderService;
            _userManager = userManager;
            _orderFacade = orderFacade;
        }

        [HttpGet("orders/history")]
        public async Task<IActionResult> OrdersHistory()
        {
            var username = User.Identity?.Name;
            if (username == null)
            {
                return BadRequest("Cannot show user history, no user is logged in.");
            }
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "An error occured while loading orders history";
                return View("ErrorMessage");
            }
            
            var orders = await _orderService.GetOrdersByUserId(user.Id);
            if (orders == null)
            {
                ViewData["ErrorMessage"] = "An error occured while loading orders history";
                return View("ErrorMessage");
            }
            
            List<OrdersHistoryViewModel> ordersHistory = orders.Select(order => new OrdersHistoryViewModel()
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Timestamp = order.Timestamp,
                State = order.State,
                Books = order.Books
            }).ToList();
            return View(ordersHistory);
        }

        [HttpGet("orders/edit")]
        public async Task<IActionResult> EditOrders()
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var orders = await _orderService.GetAllOrders();
            return View(new EditOrdersViewModel()
            {
                Orders = orders.MapToOrderViewModelCollection()
            });
        }

        [HttpGet("orders/{orderId:int}/edit")]
        public async Task<IActionResult> EditOrder(int orderId)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var orderModel = await _orderService.GetOrder(orderId);

            if (orderModel == null)
            {
                ViewData["ErrorMessage"] = "An error occured while editing order";
                return View("ErrorMessage");
            }

            return View(orderModel.MapToEditOrderViewModel());
        }

        [HttpPost("orders/{orderId:int}/edit")]
        public async Task<IActionResult> EditOrder(int orderId, EditOrderViewModel model)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editOrderModel = model.MapToEditOrderModel();

            var result = await _orderService.EditOrder(orderId, editOrderModel);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("EditOrders");
        }

        [HttpGet("orders/create")]
        public async Task<IActionResult> CreateOrder()
        {
            var user = await GetLoggedInUser();
            if (user == null)
            {
                return BadRequest("Cannot create new order when no user is logged in.");
            }
            
            var createOrderViewModel = new CreateOrderViewModel()
            {
                Email = user.Email ?? "",
                Address = "",
                Phone = 420,
            };
            return View(createOrderViewModel);
        }

        [HttpPost("orders/create")]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetLoggedInUser();
            if (user == null)
            {
                return BadRequest("Order cannot be created, no user is logged in.");
            }
            
            var createOrderModel = new CreateOrderModel()
            {
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
                State = OrderState.Ordered,
                CartId = user.CartId,
                UserId = user.Id
            };

            try
            {
                await _orderFacade.CreateOrder(createOrderModel);
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = "An error occured while creating order";
                return View("ErrorMessage");
            }
            
            return RedirectToAction("OrdersHistory");
        }
        
        private async Task<User?> GetLoggedInUser()
        {
            var username = User.Identity?.Name;
            if (username == null)
            {
                return null;
            }
        
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }
    }
}