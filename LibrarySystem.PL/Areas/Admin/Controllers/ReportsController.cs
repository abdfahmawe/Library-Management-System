using LibrarySystem.BLL.DTOs.Response.Borrowing;
using LibrarySystem.BLL.DTOs.Response.Report;
using LibrarySystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("most-borrowed-items")]
        public async Task<ActionResult<IEnumerable<MostBorrowedItemResponse>>> GetMostBorrowedItems([FromQuery]int limit =10)
        {
            IEnumerable<MostBorrowedItemResponse> borrowTransactionResponse = await _reportService.GetMostBorrowedItemsAsync(limit);
            return Ok(borrowTransactionResponse);
        }
        [HttpGet("borrowed-items-by-type")]
        public async Task<IActionResult> GetBorrowedItemsByType()
        {
            var result = await _reportService.GetBorrowedItemsByTypeAsync();

            return Ok(result);
        }
        [HttpGet("most-active-members")]
        public async Task<ActionResult<IEnumerable<MostActiveMemberResponse>>> GetMostActiveMembers([FromQuery] int limit = 10)
        {
            IEnumerable<MostActiveMemberResponse> mostActiveMembers = await _reportService.GetMostActiveMembersAsync(limit);
            return Ok(mostActiveMembers);
        }
        [HttpGet("least-borrowed-items")]
        public async Task<ActionResult<IEnumerable<LeastBorrowedItemResponse>>> GetLeastBorrowedItems([FromQuery] int limit = 10)
        {
            IEnumerable<LeastBorrowedItemResponse> leastBorrowedItems = await _reportService.GetLeastItemBorrowedAsync(limit);
            return Ok(leastBorrowedItems);
        }
        [HttpGet("Fines-over-time")]
        public async Task<ActionResult<IEnumerable<FinesOverTimeResponse>>> GetFinesOverTime()
        {
            IEnumerable<FinesOverTimeResponse> finesOverTimeResponses = await _reportService.GetFinesOverTimeAsync();
            return Ok(finesOverTimeResponses);
        }
    }
}
