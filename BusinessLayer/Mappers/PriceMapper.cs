using BusinessLayer.Models.Price;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers
{
    [Mapper]
    public static partial class PriceMapper
    {
        public static partial PriceModel MapToPriceModel(this Price price);
        public static partial List<PriceModel> MapToPriceModelList(this ICollection<Price> price);
        public static partial Price MapToPrice(this CreatePriceModel createPriceModel);
    }
}
