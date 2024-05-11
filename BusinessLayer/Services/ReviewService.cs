using BusinessLayer.Mappers;
using BusinessLayer.Models.Review;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services;

public class ReviewService : IReviewService
{
    private readonly BookHubDbContext _dbContext;

    public ReviewService(BookHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReviewModel?> CreateReviewAsync(CreateReviewModel model)
    {
        var user = await _dbContext.AppUsers.FindAsync(model.UserId);
        if (user == null)
        {
            return null;
        }

        var book = await _dbContext.Books.FindAsync(model.BookId);
        if (book == null)
        {
            return null;
        }

        var newReview = model.MapToReview();

        await _dbContext.Reviews.AddAsync(newReview);
        await SaveAsync();

        return newReview.MapToReviewModel();
    }

    public async Task<ReviewModel?> EditReviewAsync(int reviewId, EditReviewModel model)
    {
        var review = await _dbContext.Reviews
            .Where(r => r.Id == reviewId)
            .Include(r => r.User)
            .FirstOrDefaultAsync();
        
        if (review == null)
        {
            return null;
        }

        review.Rating = model.Rating ?? review.Rating;
        review.Text = model.Text ?? review.Text;
        
        await SaveAsync();

        return review.MapToReviewModel();
    }

    public async Task<ReviewModel?> GetReviewByIdAsync(int reviewId)
    {
        var reviewModel = await _dbContext.Reviews
            .Where(r => r.Id == reviewId)
            .Include(r => r.User)
            .Select(r => r.MapToReviewModel())
            .FirstOrDefaultAsync();

        return reviewModel;
    }

    public async Task<List<ReviewModel>> GetReviewsAsync()
    {
        var reviewModels = await _dbContext.Reviews
            .Include(r => r.User)
            .Select(r => r.MapToReviewModel())
            .ToListAsync();

        return reviewModels;
    }

    public async Task<List<ReviewModel>?> GetReviewsOfBookAsync(int bookId)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book == null)
        {
            return null;
        }
        
        var reviewModels = await _dbContext.Reviews
            .Where(r => r.BookId == bookId)
            .Include(r => r.User)
            .Select(r => r.MapToReviewModel())
            .ToListAsync();

        return reviewModels;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);
        if (review == null)
        {
            return false;
        }
        
        _dbContext.Reviews.Remove(review);
        await SaveAsync();

        return true;
    }
    
    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}