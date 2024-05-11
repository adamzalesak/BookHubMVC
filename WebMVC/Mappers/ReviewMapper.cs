using BusinessLayer.Models.Review;
using Riok.Mapperly.Abstractions;
using WebMVC.Models.Review;

namespace WebMVC.Mappers;

[Mapper]
public static partial class ReviewMapper
{
    public static partial EditReviewModel MapToEditReviewModel(this EditReviewViewModel model);
    public static partial CreateReviewModel MapToCreateReviewModel(this CreateReviewViewModel model);
    public static partial EditReviewViewModel MapToEditReviewViewModel(this ReviewModel model);
    public static partial List<ReviewViewModel> MapToReviewViewModelList(this List<ReviewModel> model);
}

