using LibrarySystem.BLL.DTOs.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Interfaces
{
   public interface IReportService
    {
        Task<IEnumerable<MostBorrowedItemResponse>> GetMostBorrowedItemsAsync(int limit);
        Task<IEnumerable<BorrowedItemsByTypeResponse>> GetBorrowedItemsByTypeAsync();
        Task<IEnumerable<MostActiveMemberResponse>> GetMostActiveMembersAsync(int limit);
        Task<IEnumerable<LeastBorrowedItemResponse>> GetLeastItemBorrowedAsync(int limit);

        Task<IEnumerable<FinesOverTimeResponse>> GetFinesOverTimeAsync();

    }
}
