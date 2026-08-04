using LibrarySystem.BLL.DTOs.Request.LibraryItem.Book;
using LibrarySystem.BLL.DTOs.Request.LibraryItem.Magazine;
using LibrarySystem.BLL.DTOs.Request.LibraryItem.NewsPaper;
using LibrarySystem.BLL.DTOs.Response.LibraryItem;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.Book;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.Magazine;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.NewsPaper;


namespace LibrarySystem.BLL.Services.Interfaces
{
   public enum DeleteLibraryItemResult
    {
        NotFound ,
        Deleted ,
        HasBorrowingHistory
    }
    public interface ILibraryItemService
    {
        // add book , magazaine , newspaper
        Task<BookResponse> AddBookAsync(AddBookRequest request);
        Task<MagazineResponse> AddMagazineAsync(AddMagazineRequest request);
        Task<NewspaperResponse> AddNewsPaperAsync(AddNewspaperRequest request);
        // get all library items and get by id
        Task<IEnumerable<LibraryItemResponse>> GetAllLibraryItemsAsync();
        Task<LibraryItemResponse?> GetLibraryItemByIdAsync(string id);
        // update book, magazine , newspaper
        Task<BookResponse?> UpdateBookAsync(string id, UpdateBookRequest request);
        Task<MagazineResponse?> UpdateMagazineAsync(string id, UpdateMagazineRequest request);
        Task<NewspaperResponse?> UpdateNewsPaperAsync(string id, UpdateNewspaperRequest request);
        // delete book, magazine , newspaper in one function 
        Task<DeleteLibraryItemResult> DeleteLibraryItemAsync(string id);

    }
}
