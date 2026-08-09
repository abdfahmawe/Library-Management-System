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
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MemberRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Member member)
        {
            await _dbContext.Members.AddAsync(member);
        }

        public void Delete(Member member)
        {
            _dbContext.Members.Remove(member);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
         return await _dbContext.Members.Include(m=> m.ApplicationUser).ToListAsync();
        }

        public async Task<IEnumerable<BorrowTransaction>> GetBorrowingsAsync(string membershipId)
        {
            // we nedeed the include because we want Titel and Type
           return await _dbContext.BorrowTransactions.Where(bt => bt.MembershipId == membershipId)
                .Include(bt => bt.LibraryItem)
                .OrderByDescending(bt=>bt.BorrowDate)
                .ToListAsync();
        }

        public async Task<Member?> GetByApplicationUserIdAsync(string applicationUserId)
        {
            // use FirstOrDefaultAsync ====> Not Where ===> beacuse we want only one member for one user
            return await _dbContext.Members.FirstOrDefaultAsync(member => member.ApplicationUserId == applicationUserId);
        }

        public async Task<Member?> GetByIdAsync(string membershipId)
        {
            return await _dbContext.Members.Include(m=>m.ApplicationUser).FirstOrDefaultAsync(m => m.MembershipId == membershipId);
        }

        public async Task<bool> HasBorrowTransactionsAsync(string membershipId)
        {
            return await _dbContext.BorrowTransactions.AnyAsync(bt => bt.MembershipId == membershipId);
           
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Update(Member member)
        {
            _dbContext.Members.Update(member);
        }
    }
}
