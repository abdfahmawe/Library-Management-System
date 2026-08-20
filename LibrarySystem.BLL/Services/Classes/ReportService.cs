using LibrarySystem.BLL.DTOs.Response.Report;
using LibrarySystem.BLL.Services.Interfaces;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Classes
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<BorrowedItemsByTypeResponse>> GetBorrowedItemsByTypeAsync()
        {
           IEnumerable<BorrowTransaction> borrowTransactions = await _reportRepository.GetBorrowTransactionsAsync();
            return borrowTransactions.GroupBy(br => br.LibraryItem.GetType().Name)
                .Select(group => new BorrowedItemsByTypeResponse
                {
                    Type = group.Key,
                    BorrowCount = group.Count()
                });
        }

        public async Task<IEnumerable<FinesOverTimeResponse>> GetFinesOverTimeAsync()
        {
            IEnumerable<BorrowTransaction> borrowTransactions = await _reportRepository.GetBorrowTransactionsAsync();
            return borrowTransactions.Where(borrowTransaction => borrowTransaction.IsFinePaid && borrowTransaction.Fine > 0 && borrowTransaction.ReturnDate.HasValue)
                                             .GroupBy(bt => new
                                             {
                                                 Year = bt.ReturnDate!.Value.Year,
                                                 Month = bt.ReturnDate!.Value.Month
                                             })
                                             .OrderBy(group => group.Key.Year)
                                             .ThenBy(group => group.Key.Month)
                                             .Select(group => new FinesOverTimeResponse
                                             {
                                                 Year = group.Key.Year,
                                                 Month = group.Key.Month,
                                                 TotalFines = group.Sum(bt => bt.Fine)
                                             });

        }

        public async Task<IEnumerable<LeastBorrowedItemResponse>> GetLeastItemBorrowedAsync(int limit)
        {
            IEnumerable<LibraryItem> libraryItemsWithBorrowedTransactions =await _reportRepository.GetLibraryItemsAsync();
            return libraryItemsWithBorrowedTransactions
                .OrderBy(libraryitem => libraryitem.BorrowTransactions.Count)
                .Take(limit)
                .Select(libraryitem => new LeastBorrowedItemResponse
                {
                 Title = libraryitem.Title,
                 Type = libraryitem.GetType().Name,
                 LibraryItemId = libraryitem.Id,
                 BorrowCount = libraryitem.BorrowTransactions.Count()

                });


        }

        public async Task<IEnumerable<MostActiveMemberResponse>> GetMostActiveMembersAsync(int limit)
        {
            IEnumerable<BorrowTransaction> borrowTransactions = await _reportRepository.GetBorrowTransactionsWithMembersAsync();
            return borrowTransactions.GroupBy(br => br.Member)
               .OrderByDescending(g => g.Count())
               .Take(limit)
               .Select(group => new MostActiveMemberResponse
               {
                   BorrowCount = group.Count(),
                   MembershipId = group.Key.MembershipId,
                   Name = group.Key.ApplicationUser.FullName,
               });
        }

        public async Task<IEnumerable<MostBorrowedItemResponse>> GetMostBorrowedItemsAsync(int limit)
        {
            IEnumerable<BorrowTransaction> borrowTransactions = await _reportRepository.GetBorrowTransactionsAsync();
            return borrowTransactions.GroupBy(br => br.LibraryItem)
                .OrderByDescending(g => g.Count())
                .Take(limit)
                .Select(group => new MostBorrowedItemResponse
                {
                    LibraryItemId = group.Key.Id,
                    BorrowCount = group.Count() ,
                    Title = group.Key.Title ,
                    Type = group.Key.GetType().Name
                });
        }
    }
}
