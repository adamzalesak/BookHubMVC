using BusinessLayer.Models.Order;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers
{
    [Mapper]
    public static partial class OrderMapper
    {
        public static partial OrderModel MapToOrderModel(this Order order);
        public static partial List<OrderModel> MapToOrderModelList(this ICollection<Order> order);
        public static partial Order MapToOrder(this CreateOrderModel createPriceModel);
    }

}
