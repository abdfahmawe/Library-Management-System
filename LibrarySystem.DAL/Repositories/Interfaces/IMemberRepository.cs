using LibrarySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(string membershipId);
        Task AddAsync(Member member);
        void Update(Member member);
        void Delete(Member member);
        Task<int> SaveChangesAsync();
        Task<bool> HasBorrowTransactionsAsync(string membershipId);
        Task<IEnumerable<BorrowTransaction>> GetBorrowingsAsync(string membershipId);
    }
}
