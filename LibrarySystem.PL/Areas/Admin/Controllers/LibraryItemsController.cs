using LibrarySystem.BLL.DTOs.Request.LibraryItem.Book;
using LibrarySystem.BLL.DTOs.Request.LibraryItem.Magazine;
using LibrarySystem.BLL.DTOs.Request.LibraryItem.NewsPaper;
using LibrarySystem.BLL.DTOs.Response.LibraryItem;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.Book;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.Magazine;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.NewsPaper;
using LibrarySystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [Authorize(Roles = "Admin")]
    public class LibraryItemsController : ControllerBase
    {
        private readonly ILibraryItemService _libraryItemService;

        public LibraryItemsController(ILibraryItemService libraryItemService)
        {
            _libraryItemService = libraryItemService;
        }

        [HttpPost("books")]
        public async Task<ActionResult<BookResponse>> AddBook(AddBookRequest request)
        {
            BookResponse response = await _libraryItemService.AddBookAsync(request);
            return StatusCode(StatusCodes.Status201Created , response);
        }
        [HttpPost("magazines")]
        public async Task<ActionResult<MagazineResponse>> AddMagazine(AddMagazineRequest request)
        {
            MagazineResponse response = await _libraryItemService.AddMagazineAsync(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        [HttpPost("newspapers")]
        public async Task<ActionResult<NewspaperResponse>> AddNewsPaper(AddNewspaperRequest request)
        {
            NewspaperResponse response = await _libraryItemService.AddNewsPaperAsync(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryItemResponse>>> GetAllLibraryItems()
        {
            IEnumerable<LibraryItemResponse> libraryItems = await _libraryItemService.GetAllLibraryItemsAsync();
            return Ok(libraryItems);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryItemResponse>> GetLibraryItemById([FromRoute] string id)
        {
            var libraryItem = await _libraryItemService.GetLibraryItemByIdAsync(id);
            if (libraryItem is null)
            {
                return NotFound(new
                {
                    Message = "Library item was not found."
                });
            }

            return Ok(libraryItem);

        }
        [HttpPut("books/{id}")]
        public async Task<ActionResult<BookResponse>> UpdateBookAsync([FromRoute] string id, [FromBody] UpdateBookRequest updateBookRequest)
        {
           BookResponse bookResponse = await _libraryItemService.UpdateBookAsync(id,updateBookRequest);
            if (bookResponse is null)
            {
                return NotFound(new
                {
                    Message = "Book was not found."
                });
            }

            return Ok(bookResponse);
        }
        [HttpPut("magazines/{id}")]
        public async Task<ActionResult<MagazineResponse>> UpdateMagazineAsync([FromRoute] string id, [FromBody] UpdateMagazineRequest updateMagazineRequest)
        {
            MagazineResponse magazineResponse = await _libraryItemService.UpdateMagazineAsync(id, updateMagazineRequest);
            if (magazineResponse is null)
            {
                return NotFound(new
                {
                    Message = "Magazine was not found."
                });
            }
            return Ok(magazineResponse);
        }
        [HttpPut("newspapers/{id}")]
        public async Task<ActionResult<NewspaperResponse>> UpdateNewsPaperAsync([FromRoute] string id, [FromBody] UpdateNewspaperRequest updateNewspaperRequest)
        {
            NewspaperResponse newspaperResponse = await _libraryItemService.UpdateNewsPaperAsync(id, updateNewspaperRequest);
            if (newspaperResponse is null)
            {
                return NotFound(new
                {
                    Message = "Newspaper was not found."
                });
            }
            return Ok(newspaperResponse);
        }
        // delete library item by id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteLibraryItemResult>> DeleteLibraryItemAsync([FromRoute] string id)
        {
            DeleteLibraryItemResult result = await _libraryItemService.DeleteLibraryItemAsync(id);
            return result switch
            {
                DeleteLibraryItemResult.NotFound => NotFound(new { Message = "Library item was not found." }),
                DeleteLibraryItemResult.HasBorrowingHistory => BadRequest(new { Message = "Cannot delete library item with borrowing history." }),
                DeleteLibraryItemResult.Deleted => Ok(new { Message = "Library item deleted successfully." }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." })
            };
        }
    }
}
