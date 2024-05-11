using BusinessLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Mappers;
using WebMVC.Models.Books;
using WebMVC.Models.Review;

namespace WebMVC.Controllers
{
    [Route("reviews")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateReview(CombinedBookReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editReviewModel = model.CreateReviewViewModel.MapToCreateReviewModel();

            try
            {
                await _reviewService.CreateReviewAsync(editReviewModel);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Redirect("/books/" + model.CreateReviewViewModel.BookId);
        }

        [HttpGet("{reviewId:int}/edit")]
        public async Task<IActionResult> EditReview(int reviewId)
        {
            var genreModel = await _reviewService.GetReviewByIdAsync(reviewId);

            if (genreModel == null)
            {
                return BadRequest($"Review with id {reviewId} does not exist");
            }

            return View(genreModel.MapToEditReviewViewModel());
        }

        [HttpPost("{reviewId:int}/edit")]
        public async Task<IActionResult> EditReview(int reviewId, EditReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editReviewModel = model.MapToEditReviewModel();

            try
            {
                await _reviewService.EditReviewAsync(reviewId, editReviewModel);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Redirect($"{Request.Path.ToString()}{Request.QueryString.Value.ToString()}");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(reviewId);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Redirect("~/");
        }
    }
}
