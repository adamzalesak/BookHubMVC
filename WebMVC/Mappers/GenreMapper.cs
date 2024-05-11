using WebMVC.Models;
using BusinessLayer.Models.Genre;
using Riok.Mapperly.Abstractions;

namespace WebMVC.Mappers;

[Mapper]
public static partial class GenreMapper
{
    public static partial EditGenreModel MapToEditGenreModel(this EditGenreViewModel model);
    public static partial EditGenreViewModel MapToEditGenreViewModel(this GenreModel model);
}