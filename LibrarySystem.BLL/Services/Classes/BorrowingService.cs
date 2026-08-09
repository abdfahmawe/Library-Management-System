using LibrarySystem.BLL.Common.Enums;
using LibrarySystem.BLL.DTOs.Response.Borrowing;
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
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowTransactionRepository _borrowTransactionRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILibraryItemRepository _libraryItemRepository;
        private const int BorrowDurationDays = 14;
        private const decimal FinePerLateDay = 1m; // 1$ per day

        public BorrowingService(IBorrowTransactionRepository borrowTransactionRepository , 
                                IMemberRepository memberRepository  ,
                                ILibraryItemRepository libraryItemRepository)
        {
            _borrowTransactionRepository = borrowTransactionRepository;
            _memberRepository = memberRepository;
            _libraryItemRepository = libraryItemRepository;
        }

        public async Task<BorrowResult> BorrowAsync(string applicationUserId, string libraryItemId)
        {
            Member? member = await _memberRepository.GetByApplicationUserIdAsync(applicationUserId);
            if (member is null)
            {

                return new BorrowResult
                {
                    Status = BorrowStatus.MemberNotFound,

                };

            }
            LibraryItem? libraryItem = await _libraryItemRepository.GetByIdAsync(libraryItemId);
            if (libraryItem is null)
            {
                return new BorrowResult
                {
                    Status = BorrowStatus.LibraryItemNotFound,
                };
            }
            if(libraryItem.IsAvailable == false)
            {
                return new BorrowResult
                {
                    Status = BorrowStatus.NotAvailable,
                };
            }
            libraryItem.IsAvailable = false;
            DateTime borrowDate = DateTime.UtcNow;
            BorrowTransaction borrowTransaction = new BorrowTransaction
            {
                BorrowDate = borrowDate,
                DueDate = borrowDate.AddDays(BorrowDurationDays),
                ReturnDate = null,
                Fine = 0,
                IsFinePaid = true,
                MembershipId = member.MembershipId,
                LibraryItemId = libraryItem.Id
            };
            await _borrowTransactionRepository.AddAsync(borrowTransaction);
            await _borrowTransactionRepository.SaveChangesAsync();
            // now response => no need update libraryItem because   libraryItem.IsAvailable = false; is already updated in the database because of the tracking of the entity framework
            BorrowTransactionResponse borrowTransactionResponse = new BorrowTransactionResponse
            {
                BorrowDate = borrowTransaction.BorrowDate,
                DueDate = borrowTransaction.DueDate,
                Id = borrowTransaction.Id,
                LibraryItemId = borrowTransaction.LibraryItemId,
                LibraryItemTitle = libraryItem.Title
            };
            return new BorrowResult
            {
                Status = BorrowStatus.Borrowed,
                BorrowTransaction = borrowTransactionResponse
            }; 
        }

        public async Task<ReturnResult> ReturnAsync(string applicationUserId, string borrowTransactionId)
        {
            Member? member = await _memberRepository.GetByApplicationUserIdAsync(applicationUserId);
            if (member is null)
            {
                return new ReturnResult
                {
                    Status = ReturnStatus.MemberNotFound
                };
            }
            BorrowTransaction? borrowTransaction = await _borrowTransactionRepository.GetByIdAndMembershipIdAsync(borrowTransactionId, member.MembershipId);
            if(borrowTransaction is null)
            {
                return new ReturnResult
                {
                    Status = ReturnStatus.BorrowTransactionNotFound
                };
            }
            if (borrowTransaction.ReturnDate is not null)
            {
                return new ReturnResult
                {
                    Status = ReturnStatus.AlreadyReturned
                };
            }
            DateTime returnDate = DateTime.UtcNow;
            borrowTransaction.ReturnDate = returnDate;
           if(returnDate.Date > borrowTransaction.DueDate.Date)
            {
                int late = (returnDate.Date - borrowTransaction.DueDate.Date).Days; // أول اشي بحولها لايام اخر اشي فنكشن ال Days بحولهن لرقم 
                borrowTransaction.Fine = late * FinePerLateDay; // 1$ per day
                borrowTransaction.IsFinePaid = false;
            }
            else
            {
                borrowTransaction.Fine = 0; 
                borrowTransaction.IsFinePaid = true;
            }
           // now return the status to Avalible 
           borrowTransaction.LibraryItem.IsAvailable = true;
            await _borrowTransactionRepository.SaveChangesAsync();
            ReturnTransactionResponse returnTransactionResponse = new ReturnTransactionResponse
            {
                Id = borrowTransaction.Id,
                LibraryItemId = borrowTransaction.LibraryItemId,
                LibraryItemTitle = borrowTransaction.LibraryItem.Title,
                BorrowDate = borrowTransaction.BorrowDate,
                DueDate = borrowTransaction.DueDate,
                ReturnDate = borrowTransaction.ReturnDate.Value,
                Fine = borrowTransaction.Fine,
                IsFinePaid = borrowTransaction.IsFinePaid
            };
            return new ReturnResult
            {
                Status = ReturnStatus.Returned,
                Transaction = returnTransactionResponse
            };
        }
    }
}
