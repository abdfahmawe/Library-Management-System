using LibrarySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Repositories.Interfaces
{
    public interface IBorrowTransactionRepository
    {
        Task AddAsync(BorrowTransaction borrowTransaction);
        Task<int> SaveChangesAsync();
        Task<BorrowTransaction?> GetByIdAndMembershipIdAsync(string TransactionId, string membershipId);
    }
}
