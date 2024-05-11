using BusinessLayer.Models.Review;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers;

[Mapper]
public static partial class ReviewMapper
{
    public static partial Review MapToReview(this CreateReviewModel model);
    
    [MapProperty(nameof(Review.User), nameof(ReviewModel.Username))]
    public static partial ReviewModel MapToReviewModel(this Review review);
    
    private static string UserToUsername(User user) => user.UserName;
}