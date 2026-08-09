using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Repositories.Classes
{
    public class BorrowTransactionRepository : IBorrowTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BorrowTransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(BorrowTransaction borrowTransaction)
        {
           await _dbContext.BorrowTransactions.AddAsync(borrowTransaction);
        }

        public async Task<BorrowTransaction?> GetByIdAndMembershipIdAsync(string TransactionId, string membershipId)
        {
           return await _dbContext.BorrowTransactions.Include(t=> t.LibraryItem)
                                                    .FirstOrDefaultAsync(t => t.Id == TransactionId && t.MembershipId == membershipId); 
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }
    }
}
