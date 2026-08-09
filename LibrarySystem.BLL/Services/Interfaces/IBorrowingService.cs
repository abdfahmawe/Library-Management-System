using LibrarySystem.BLL.DTOs.Response.Borrowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<BorrowResult> BorrowAsync(string applicationUserId, string libraryItemId);
        Task<ReturnResult> ReturnAsync(string applicationUserId, string borrowTransactionId);
    }
}
