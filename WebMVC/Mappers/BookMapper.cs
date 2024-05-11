using BusinessLayer.Models.Book;
using Riok.Mapperly.Abstractions;
using WebMVC.Models;

namespace WebMVC.Mappers;

[Mapper]
public static partial class BookMapper
{
    public static partial BookViewModel MapToBookViewModel(this BookModel model);
    public static partial EditBookModel MapToEditBookModel(this EditBookViewModel model);
    public static partial EditBookViewModel MapToEditBookViewModel(this BookModel model);
}