using LibrarySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Repositories.Interfaces
{
    public interface ILibraryItemRepository
    {
        // get all
         Task<IEnumerable<LibraryItem>> GetAllAsync();
        // get by id
         Task<LibraryItem?> GetByIdAsync(string id);
        // add 
         Task AddAsync(LibraryItem libraryItem);
        // update  => with no Task Because we dont update the data base until we call SaveChangesAsync  
        void Update(LibraryItem libraryItem);
        //delete  => with no Task Because we dont update the data base until we call SaveChangesAsync
        void Delete(LibraryItem libraryItem);
        Task<int> SaveChangesAsync();

        Task<bool> HasBorrowTransactionsAsync(string id);

        // for member to do filtering 
        Task<IEnumerable<LibraryItem>> SearchAvailableAsync(string? title,
    string? author,
    int? year,
    string? type);
    }
}
