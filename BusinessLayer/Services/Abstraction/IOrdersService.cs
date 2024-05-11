using BusinessLayer.Models.Order;

namespace BusinessLayer.Services.Abstraction
{
    public interface IOrdersService : IBaseService
    {
        public Task<OrderModel?> GetOrder(int id);
        public Task<List<OrderModel>> GetAllOrders();
        public Task<OrderModel?> CreateOrder(CreateOrderModel orderDto);
        public Task<bool> EditOrder(int id, EditOrderModel orderDto);
        public Task<bool> DeleteOrder(int id);
        public Task<List<OrderModel>?> GetOrdersInInterval(DateTime from, DateTime to);
        public Task<List<UserOrdersModel>?> GetOrdersByUserId(String userId);
    }
}
