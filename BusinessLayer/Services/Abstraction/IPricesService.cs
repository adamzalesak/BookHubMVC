using BusinessLayer.Models.Price;

namespace BusinessLayer.Services.Abstraction
{
    public interface IPricesService : IBaseService
    {
        public Task<PriceModel?> GetPrice(int id);
        public Task<List<PriceModel>> GetAllPrices();
        public Task<PriceModel?> CreatePrice(CreatePriceModel priceDto);
        public Task<bool> DeletePrice(int id);
        public Task<List<PriceModel>> FindBookHistoryPrices(int bookId);
    }
}
