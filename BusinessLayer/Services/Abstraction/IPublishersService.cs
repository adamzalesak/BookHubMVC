using BusinessLayer.Models.Publisher;

namespace BusinessLayer.Services.Abstraction;

public interface IPublishersService : IBaseService
{
    public Task<ICollection<PublisherModel>> GetPublishersAsync(string? filterName = null);
    public Task<PublisherModel?> GetPublisherByIdAsync(int id);
    public Task<PublisherModel?> EditPublisherAsync(int id, EditPublisherModel model);
}