using BusinessLayer.Mappers;
using BusinessLayer.Models.Publisher;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services;

public class PublishersService : IPublishersService
{
    private readonly BookHubDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    
    public PublishersService(BookHubDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<ICollection<PublisherModel>> GetPublishersAsync(string? filterName = null)
    {
        var filterNameString = filterName?.ToLower() ?? "";
        
        if (_memoryCache.TryGetValue(Constants.GetPublishersCacheKey + filterNameString, out List<PublisherModel>? publishers) && publishers != null)
        {
            return publishers;
        }

        var publishersFromDb = await _dbContext.Publishers
            .Where(p => p.Name.ToLower().Contains(filterNameString))
            .Select(p => p.MapToPublisherModel())
            .ToListAsync();

        _memoryCache.Set(Constants.GetPublishersCacheKey + filterNameString, publishersFromDb,
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(3)));

        return publishersFromDb;
    }
    
    public async Task<PublisherModel?> GetPublisherByIdAsync(int id)
    {
        var publisher = await _dbContext.Publishers
            .FirstOrDefaultAsync(p => p.Id == id);
        
        return publisher?.MapToPublisherModel();
    }
    
    public async Task<PublisherModel?> EditPublisherAsync(int reviewId, EditPublisherModel model)
    {
        var publisher = await _dbContext.Publishers
            .Where(r => r.Id == reviewId)
            .FirstOrDefaultAsync();
        
        if (publisher == null)
        {
            return null;
        }

        publisher.Name = model.Name ?? publisher.Name;
        publisher.Description = model.Description ?? publisher.Description;
        
        await SaveAsync();
        
        ClearCache();

        return publisher.MapToPublisherModel();
    }

    public Task SaveAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
    
    private void ClearCache()
    {
        _memoryCache.Remove(Constants.GetPublishersCacheKey);
    }
}