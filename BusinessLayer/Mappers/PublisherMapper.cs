using BusinessLayer.Models.Publisher;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers;

[Mapper]
public static partial class PublisherMapper
{
    public static partial PublisherModel MapToPublisherModel(this Publisher genre);
}