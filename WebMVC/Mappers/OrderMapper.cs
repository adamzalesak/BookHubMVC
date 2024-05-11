using WebMVC.Models;
using BusinessLayer.Models.Order;
using Riok.Mapperly.Abstractions;
using WebMVC.Views.Orders;

namespace WebMVC.Mappers;

[Mapper]
public static partial class OrderMapper
{
    public static partial EditOrderModel MapToEditOrderModel(this EditOrderViewModel model);
    public static partial EditOrderViewModel MapToEditOrderViewModel(this OrderModel model);
    public static partial ICollection<OrderViewModel> MapToOrderViewModelCollection(this ICollection<OrderModel> models);
}