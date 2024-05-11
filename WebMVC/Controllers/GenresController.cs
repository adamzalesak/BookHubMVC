using BusinessLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Mappers;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Route("genres")]
public class GenresController : Controller
{
    private readonly IGenreService _genreService;
    
    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }
    
    [HttpGet("{genreId:int}/edit")]
    public async Task<IActionResult> EditGenre(int genreId)
    {
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized();
        }
        
        var genreModel = await _genreService.GetGenreByIdAsync(genreId);

        if (genreModel == null)
        {
            return BadRequest($"Genre with id {genreId} does not exist");
        }
        
        return View(genreModel.MapToEditGenreViewModel());
    }
    
    [HttpPost("{genreId:int}/edit")]
    public async Task<IActionResult> EditGenre(int genreId, EditGenreViewModel model)
    {
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized();
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var editGenreModel = model.MapToEditGenreModel();

        try
        {
            await _genreService.EditGenreAsync(genreId, editGenreModel);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

        return Redirect("~/");
    }
}