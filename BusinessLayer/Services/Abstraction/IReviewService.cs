using BusinessLayer.Models.Review;

namespace BusinessLayer.Services.Abstraction;

public interface IReviewService : IBaseService
{
    public Task<ReviewModel?> CreateReviewAsync(CreateReviewModel model);
    public Task<ReviewModel?> EditReviewAsync(int reviewId, EditReviewModel model);
    public Task<List<ReviewModel>> GetReviewsAsync();
    public Task<ReviewModel?> GetReviewByIdAsync(int reviewId);
    public Task<List<ReviewModel>?> GetReviewsOfBookAsync(int bookId);
    public Task<bool> DeleteReviewAsync(int reviewId);
}