using BusinessLayer.Mappers;
using BusinessLayer.Models.Order;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly BookHubDbContext _dbContext;

        public OrdersService(BookHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderModel?> CreateOrder(CreateOrderModel orderDto)
        {
            var cart = await _dbContext.Carts.FindAsync(orderDto.CartId);
            if (cart == null)
            {
                return null;
            }
            var order = orderDto.MapToOrder();
            order.Timestamp = DateTime.Now;
            order.Cart = cart;
            
            if (orderDto.UserId != null)
            {
                order.UserId = orderDto.UserId;
                order.User = await _dbContext.AppUsers.SingleOrDefaultAsync(u => u.Id == orderDto.UserId);
            }

            var newOrder = await _dbContext.Orders.AddAsync(order);
            await SaveAsync();
            return newOrder.Entity.MapToOrderModel();
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _dbContext.Orders.Remove(order);
            await SaveAsync();
            return true;
        }

        public async Task<bool> EditOrder(int id, EditOrderModel orderDto)
        {
            var order = await _dbContext.Orders
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        
            if (order == null)
            {
                return false;
            }

            order.Email = orderDto.Email ?? order.Email;
            order.Address = orderDto.Address ?? order.Address;
            order.Phone = orderDto.Phone ?? order.Phone;
            order.State = orderDto.State ?? order.State;
        
            await SaveAsync();

            return true;
        }

        public async Task<List<OrderModel>?> GetOrdersInInterval(DateTime from, DateTime to)
        {
            var orders = await _dbContext.Orders
            .Include(o => o.Cart)
            .Include(o => o.User)
            .Where(o => o.Timestamp > from && o.Timestamp < to).ToListAsync();
            return orders.MapToOrderModelList();
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            var orders = await _dbContext.Orders
            .Include(o => o.Cart)
            .Include(o => o.User)
            .ToListAsync();
            return orders.MapToOrderModelList();
        }

        public async Task<OrderModel?> GetOrder(int id)
        {
            Order? order = await GetOrderObject(id);
            if (order == null)
            {
                return null;
            }
            return order.MapToOrderModel();
        }

        private async Task<Order?> GetOrderObject(int id)
        {
            return await _dbContext.Orders
            .Include(o => o.Cart)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserOrdersModel>?> GetOrdersByUserId(String userId)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .ToListAsync();
            var models = orders.Select(order => new UserOrdersModel()
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Timestamp = order.Timestamp,
                State = order.State,
                Books = order.Cart.CartItems.Select(ci => ci.Book.Name).ToList()
            }).ToList();
            return models;
        }
    }
}
