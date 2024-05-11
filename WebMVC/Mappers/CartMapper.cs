using BusinessLayer.Models.Cart;
using Riok.Mapperly.Abstractions;
using WebMVC.Models.Cart;

namespace WebMVC.Mappers;

[Mapper]
public static partial class CartMapper
{
    public static partial CartViewModel MapToCartViewModel(this DetailedCartModel model); 
    private static partial CartItemViewModel MapToCartItemViewModel(this DetailedCartItemModel model);
}