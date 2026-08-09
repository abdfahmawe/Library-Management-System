using LibrarySystem.BLL.Common.Enums;
using LibrarySystem.BLL.DTOs.Response.Borrowing;
using LibrarySystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrarySystem.PL.Areas.Member.Controllers
{
    [Area("Member")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Member")]
    public class BorrowingsController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingsController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }
        [HttpPost("{libraryItemId}")]
        public async Task<ActionResult<BorrowTransactionResponse>> Borrow([FromRoute] string libraryItemId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            BorrowResult borrowResult = await _borrowingService.BorrowAsync(userId, libraryItemId);
            return borrowResult.Status switch
            {
                BorrowStatus.Borrowed =>
                    StatusCode(StatusCodes.Status201Created, borrowResult.BorrowTransaction),

                BorrowStatus.MemberNotFound =>
                    NotFound("Member profile was not found."),

                BorrowStatus.LibraryItemNotFound =>
                    NotFound("Library item was not found."),

                BorrowStatus.NotAvailable =>
                    Conflict("Library item is not available."),

                _ => StatusCode(500)
            };
        }

        [HttpPost("{borrowTransactionId}/return")]
        public async Task<ActionResult<ReturnTransactionResponse>> Return([FromRoute] string borrowTransactionId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            ReturnResult returnResult = await _borrowingService.ReturnAsync(userId, borrowTransactionId);
            return returnResult.Status switch
            {
                ReturnStatus.Returned =>
                    Ok(returnResult.Transaction),

                ReturnStatus.MemberNotFound =>
                    NotFound("Member profile was not found."),

                ReturnStatus.BorrowTransactionNotFound =>
                    NotFound("Borrow transaction was not found."),

                ReturnStatus.AlreadyReturned =>
                    Conflict("This item has already been returned."),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
