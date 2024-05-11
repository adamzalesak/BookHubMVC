using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Mappers;
using WebMVC.Models;
using WebMVC.Models.Publishers;

namespace WebMVC.Controllers;

[Route("publishers")]
public class PublishersController : Controller
{
    private readonly IPublishersService _publishersService;
    
    public PublishersController(IPublishersService publishersService)
    {
        _publishersService = publishersService;
    }
    
    [HttpGet("{publisherId:int}/edit")]
    public async Task<IActionResult> EditPublisher(int publisherId)
    {
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized();
        }
        
        var publisherModel = await _publishersService.GetPublisherByIdAsync(publisherId);

        if (publisherModel == null)
        {
            return BadRequest($"Publisher with id {publisherId} does not exist");
        }
        
        return View(publisherModel.MapToEditPublisherViewModel());
    }
    
    [HttpPost("{publisherId:int}/edit")]
    public async Task<IActionResult> EditPublisher(int publisherId, EditPublisherViewModel model)
    {
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized();
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var editPublisherModel = model.MapToEditPublisherModel();

        try
        {
            await _publishersService.EditPublisherAsync(publisherId, editPublisherModel);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

        return Redirect("~/");
    }
}