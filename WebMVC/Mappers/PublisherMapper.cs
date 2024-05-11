using WebMVC.Models;
using BusinessLayer.Models.Publisher;
using Riok.Mapperly.Abstractions;
using WebMVC.Models.Publishers;

namespace WebMVC.Mappers;

[Mapper]
public static partial class PublisherMapper
{
    public static partial EditPublisherModel MapToEditPublisherModel(this EditPublisherViewModel model);
    public static partial EditPublisherViewModel MapToEditPublisherViewModel(this PublisherModel model);
}