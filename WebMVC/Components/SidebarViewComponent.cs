using BusinessLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Components;

public class SidebarViewComponent : ViewComponent
{
    private readonly IGenreService _genreService;
    private readonly IPublishersService _publisherService;
    
    public SidebarViewComponent(IGenreService genreService, IPublishersService publisherService)
    {
        _genreService = genreService;
        _publisherService = publisherService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var genres = await _genreService.GetGenresAsync();
        var publishers = await _publisherService.GetPublishersAsync();
        
        var model = new SidebarViewModel
        {
            Genres = genres,
            Publishers = publishers,
        };
        
        return View(model);
    }
}