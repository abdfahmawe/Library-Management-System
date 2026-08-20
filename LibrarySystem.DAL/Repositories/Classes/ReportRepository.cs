using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace LibrarySystem.DAL.Repositories.Classes
{
   public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BorrowTransaction>> GetBorrowTransactionsAsync()
        {
            return await _dbContext.BorrowTransactions.Include(transaction => transaction.LibraryItem).ToListAsync();
        }

        public async Task<IEnumerable<BorrowTransaction>> GetBorrowTransactionsWithMembersAsync()
        {
            return await _dbContext.BorrowTransactions.Include(bt => bt.Member)
                 .ThenInclude(member => member.ApplicationUser).ToListAsync();
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsAsync()
        {
            return await _dbContext.LibraryItems.Include(libraryitem => libraryitem.BorrowTransactions).ToListAsync();
        }

    }
}
