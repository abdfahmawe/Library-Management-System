using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace LibrarySystem.DAL.Repositories.Classes
{
    public class LibraryItemRepository : ILibraryItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LibraryItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(LibraryItem libraryItem)
        {
            await _dbContext.LibraryItems.AddAsync(libraryItem);
         
        }

        public void  Delete(LibraryItem libraryItem)
        {
            // the task of Delete is not Async because it change the entity in the (Change Tracker)
            _dbContext.LibraryItems.Remove(libraryItem);
           
        }

        public async Task<IEnumerable<LibraryItem>> GetAllAsync()
        {
            return await _dbContext.LibraryItems.ToListAsync();
             
        }

        public async Task<LibraryItem?> GetByIdAsync(string id)
        {
            return await _dbContext.LibraryItems.FirstOrDefaultAsync(libItem => libItem.Id == id);
          
        }

        public async Task<bool> HasBorrowTransactionsAsync(string id)
        {
            return await _dbContext.BorrowTransactions.AnyAsync(trans => trans.LibraryItemId == id);

        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Update(LibraryItem libraryItem)
        {
           _dbContext.LibraryItems.Update(libraryItem);
          
        }
    }
}
