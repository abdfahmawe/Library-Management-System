using LibrarySystem.BLL.DTOs.Request.Catalog;
using LibrarySystem.BLL.DTOs.Response.Catalog;
using LibrarySystem.BLL.DTOs.Response.LibraryItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Interfaces
{
    public interface IMemberCatalogService
    {
        Task<IEnumerable<CatalogItemResponse>> GetCatalogAsync(
         CatalogFilterRequest request);

        Task<LibraryItemDetailsResponse?> GetLibraryItemByIdAsync(string id);
    }
}
