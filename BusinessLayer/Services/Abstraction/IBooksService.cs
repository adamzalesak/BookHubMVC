using BusinessLayer.Models.Book;

namespace BusinessLayer.Services.Abstraction;

public interface IBooksService : IBaseService
{
    public Task<BookPaginationModel> GetBooksAsync(GetBooksModel parameters);
    public Task<BookModel?> GetBookAsync(int bookId);
    public Task<BookModel> CreateBookAsync(CreateBookModel model);
    public Task EditBookAsync(int bookId, EditBookModel model);
    public Task DeleteBookAsync(int bookId);
}