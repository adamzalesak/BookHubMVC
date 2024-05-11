using BusinessLayer.Models.Genre;
using DataAccessLayer.Models;
using Riok.Mapperly.Abstractions;

namespace BusinessLayer.Mappers;

[Mapper]
public static partial class GenreMapper
{
    public static partial Genre MapToGenre(this CreateGenreModel model);

    public static partial GenreModel MapToGenreModel(this Genre genre);
}