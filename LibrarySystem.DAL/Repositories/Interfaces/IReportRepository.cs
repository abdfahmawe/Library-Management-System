using LibrarySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<BorrowTransaction>> GetBorrowTransactionsWithMembersAsync();
        Task<IEnumerable<BorrowTransaction>> GetBorrowTransactionsAsync();
        Task<IEnumerable<LibraryItem>> GetLibraryItemsAsync();
    }
}
