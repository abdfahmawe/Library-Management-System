using LibrarySystem.BLL.DTOs.Request.Catalog;
using LibrarySystem.BLL.DTOs.Response.Catalog;
using LibrarySystem.BLL.DTOs.Response.LibraryItem;
using LibrarySystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Areas.Member.Controllers
{
    [Area("Member")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Member")]
    public class CatalogController : ControllerBase
    {
        private readonly IMemberCatalogService _memberCatalogService;

        public CatalogController(IMemberCatalogService memberCatalogService)
        {
            _memberCatalogService = memberCatalogService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogItemResponse>>> GetCatalog([FromQuery]CatalogFilterRequest request)
        {
            IEnumerable<CatalogItemResponse> catalogItems = await _memberCatalogService.GetCatalogAsync(request);
            return Ok(catalogItems);
        }
        [HttpGet("{libraryItemId}")]
        public async Task<ActionResult<LibraryItemDetailsResponse>> GetById([FromRoute] string libraryItemId)
        {
            LibraryItemDetailsResponse? libraryItem =await _memberCatalogService.GetLibraryItemByIdAsync(libraryItemId);
            if (libraryItem is null)
            {
                return NotFound();
            }
            return Ok(libraryItem);
        }
    }
}
