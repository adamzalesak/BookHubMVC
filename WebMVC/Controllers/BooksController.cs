using System;
using System.Diagnostics;
using BusinessLayer.Models.Book;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Mappers;
using WebMVC.Models;
using WebMVC.Models.Books;
using WebMVC.Models.Review;
using BookMapper = WebMVC.Mappers.BookMapper;

namespace WebMVC.Controllers;

public class BooksController : Controller
{
    private readonly IBooksService _booksService;
    private readonly IGenreService _genresService;
    private readonly IPublishersService _publishersService;
    private readonly IReviewService _reviewService;
    private readonly UserManager<User> _userManager;


    public BooksController(IBooksService booksService,
        IGenreService genresService,
        IPublishersService publishersService,
        IReviewService reviewService,
        UserManager<User> userManager)
    {
        _booksService = booksService;
        _genresService = genresService;
        _publishersService = publishersService;
        _reviewService = reviewService;
        _userManager = userManager;
    }


    [Route("")]
    [Route("books")]
    public async Task<ActionResult> List([FromQuery] int? genreId, [FromQuery] int? publisherId, string? searchString, [FromQuery] int? pageIndex = 1, [FromQuery] int? pageSize = 10)
    {
        var books = await _booksService.GetBooksAsync(new GetBooksModel
        {
            GenreId = genreId,
            PublisherId = publisherId,
            Page = pageIndex - 1,
            PageSize = pageSize,
            Name = searchString,
        });
        var bookModels = books.Books.Select(BookMapper.MapToBookViewModel).ToList();

        var genre = genreId is null ? null : await _genresService.GetGenreByIdAsync(genreId.Value);
        var publisher = publisherId is null ? null : await _publishersService.GetPublisherByIdAsync(publisherId.Value);
        
        var foundGenres = searchString is null ? null : await _genresService.GetGenresAsync(searchString);
        var foundPublishers = searchString is null ? null : await _publishersService.GetPublishersAsync(searchString);
        var pageCount = books.TotalCount % pageSize == 0 ? books.TotalCount / pageSize : (books.TotalCount / pageSize) + 1;

        var model = new ListBooksViewModel
        {
            Books = bookModels,
            FilteredGenreName = genre?.Name,
            FilteredPublisherName = publisher?.Name,
            FoundGenres = foundGenres,
            FoundPublishers = foundPublishers,
            PageCount = (int)Math.Round((decimal)pageCount),
            PageIndex = books.PageIndex,
            PageSize = (int)pageSize,
            GenreId = genreId,
            PublisherId = publisherId,
            SearchString = searchString
        };

        return View(model);
    }

    [Route("books/{bookId:int}")]
    public async Task<ActionResult> Detail(int bookId)
    {
        var book = await _booksService.GetBookAsync(bookId);

        if (book is null)
        {
            return NotFound();
        }

        var bookModel = book.MapToBookViewModel();
        var reviews = await _reviewService.GetReviewsOfBookAsync(bookId);
        if (reviews.Count > 0)
        {
            bookModel.Reviews = reviews.MapToReviewViewModelList();
        }
        var combinedModel = new CombinedBookReviewViewModel();
        combinedModel.BookViewModel = bookModel;
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var createReviewModel = new CreateReviewViewModel();
            createReviewModel.BookId = bookId;
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            createReviewModel.UserId = user.Id;
            combinedModel.CreateReviewViewModel = createReviewModel;
        }         
        
        return View(combinedModel);
    }
    
    [HttpGet("books/{bookId:int}/edit")]
    public async Task<IActionResult> EditBook(int bookId)
    {
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized();
        }
        
        var bookModel = await _booksService.GetBookAsync(bookId);

        if (bookModel == null)
        {
            return BadRequest($"Book with id {bookId} does not exist");
        }
        
        return View(bookModel.MapToEditBookViewModel());
    }
    
    [HttpPost("books/{bookId:int}/edit")]
    public async Task<IActionResult> EditBook(int bookId, EditBookViewModel model)
    {
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized();
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var editBookModel = model.MapToEditBookModel();

        try
        {
            await _booksService.EditBookAsync(bookId, editBookModel);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

        return Redirect("~/");
    }
}