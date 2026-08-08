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

        public async Task<IEnumerable<LibraryItem>> SearchAvailableAsync(string? title, string? author, int? year, string? type)
        {
           IQueryable<LibraryItem> query = _dbContext.LibraryItems.Where(item => item.IsAvailable == true);
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(item => item.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(item => item.Author.Contains(author));
            }
            if (year.HasValue)
            {
                query = query.Where(item => item.YearOfPublication == year.Value);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(item => EF.Property<string>(item, "LibraryItemType") == type);
            }
            return await query.ToListAsync();
        }

        public void Update(LibraryItem libraryItem)
        {
           _dbContext.LibraryItems.Update(libraryItem);
          
        }
    }
}
