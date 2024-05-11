using BusinessLayer.Models.Genre;

namespace BusinessLayer.Services.Abstraction;

public interface IGenreService : IBaseService
{
    public Task<GenreModel> CreateGenreAsync(CreateGenreModel model);
    public Task<GenreModel?> EditGenreAsync(int genreId, EditGenreModel model);
    public Task<List<GenreModel>> GetGenresAsync(string? filterName = null);
    public Task<GenreModel?> GetGenreByIdAsync(int genreId);
    public Task<bool> DeleteGenreAsync(int genreId);
}