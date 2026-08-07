using LibrarySystem.BLL.Common.Enums;
using LibrarySystem.BLL.DTOs.Request.Member;
using LibrarySystem.BLL.DTOs.Response.Member;
using LibrarySystem.BLL.Services.Interfaces;
using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace LibrarySystem.BLL.Services.Classes
{
   public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public MemberService(IMemberRepository memberRepository ,
            UserManager<ApplicationUser> userManager ,
            ApplicationDbContext dbContext)
        {
            _memberRepository = memberRepository;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<AddMemberResult> AddMemberAsync(AddMemberRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                return new AddMemberResult
                {
                    Status = AddMemberStatus.EmailAlreadyExists
                };
            }
            ApplicationUser? userByApplicationUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByApplicationUserName is not null)
            {
                return new AddMemberResult
                {
                    Status = AddMemberStatus.UserNameAlreadyExists
                };
            }
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(); //  await using var know why 
            try
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    FullName = request.FullName,
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };
                IdentityResult result = await _userManager.CreateAsync(applicationUser, request.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new AddMemberResult
                    {
                        Status = AddMemberStatus.CreationFailed,
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                IdentityResult addRoleResult =
                    await _userManager.AddToRoleAsync(
                        applicationUser,
                        "Member");

                if (!addRoleResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new AddMemberResult
                    {
                        Status = AddMemberStatus.CreationFailed,

                        Errors = addRoleResult.Errors
                            .Select(error => error.Description)
                    };
                }
                Member member = new Member
                {
                    MembershipId = Guid.NewGuid().ToString(),
                    ApplicationUserId = applicationUser.Id
                };
                await _memberRepository.AddAsync(member);
                await _memberRepository.SaveChangesAsync();

                await transaction.CommitAsync();
                MemberResponse memberResponse = new MemberResponse
                {
                    MembershipId = member.MembershipId,
                    ApplicationUserId = applicationUser.Id,
                    UserName = applicationUser.UserName!,
                    FullName = applicationUser.FullName,
                    Email = applicationUser.Email!,
                    PhoneNumber = applicationUser.PhoneNumber
                };

                return new AddMemberResult
                {
                    Status = AddMemberStatus.Created,
                    Member = memberResponse
                };

            }
            catch
            {
                await transaction.RollbackAsync();
                return new AddMemberResult
                {
                    Status = AddMemberStatus.CreationFailed,
                    Errors = new List<string> { "An error occurred while creating the member." }
                };
            }
        }

        public async Task<DeleteMemberResult> DeleteMemberAsync(string membershipId)
        {
            Member? member = await _memberRepository.GetByIdAsync(membershipId);
            if (member == null)
            {
                return DeleteMemberResult.NotFound;
            }
            bool hasBorrowTransactions = await _memberRepository.HasBorrowTransactionsAsync(membershipId);
            if (hasBorrowTransactions)
            {
                return DeleteMemberResult.HasBorrowingHistory;
            }
            ApplicationUser applicationUser = member.ApplicationUser;
            await using var transaction =
                 await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _memberRepository.Delete(member);
                await _memberRepository.SaveChangesAsync();
                // عملية ثانية تحفظ داخلياً عبر نفس DbContext/connection
                IdentityResult result =
                    await _userManager.DeleteAsync(applicationUser);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return DeleteMemberResult.DeleteFailed;
                }
                await transaction.CommitAsync();
                return DeleteMemberResult.Deleted;

            }
            catch 
            {
                await transaction.RollbackAsync();
                throw;
            }


        }

        public async Task<IEnumerable<MemberResponse>> GetAllMembersAsync()
        {
            IEnumerable<Member> members = await _memberRepository.GetAllAsync();
            // we used manual mapping not mapspter because there is 2 source of the data
            // application user and member table so we need to map them manually
            // ApplicationUserId , MembershipId => from member table
            // FullName, Email, PhoneNumber => from application user table
            return members.Select(member => new MemberResponse
            {
                ApplicationUserId = member.ApplicationUserId,
                Email = member.ApplicationUser.Email!,
                FullName = member.ApplicationUser.FullName,
                MembershipId = member.MembershipId,
                UserName = member.ApplicationUser.UserName!,
                PhoneNumber = member.ApplicationUser.PhoneNumber
            });
        }

        public async Task<IEnumerable<MemberBorrowingResponse>?> GetMemberBorrowingsAsync(string membershipId)
        {
            Member? member = await _memberRepository.GetByIdAsync(membershipId);
            if (member is null)
            {
                return null;
            }
            IEnumerable<BorrowTransaction> borrowTransactions = await _memberRepository.GetBorrowingsAsync(membershipId);
            // why we use manual mapping ????? 
            // because data come from two 2 sources ... => 1- borrow transaction   2- library item 
            return borrowTransactions.Select(bt => new MemberBorrowingResponse
            {
                BorrowTransactionId = bt.Id,

                LibraryItemId = bt.LibraryItemId,

                Title = bt.LibraryItem.Title,

                Type = bt.LibraryItem.GetType().Name,

                BorrowDate = bt.BorrowDate,

                DueDate = bt.DueDate,

                ReturnDate = bt.ReturnDate,

                Fine = bt.Fine,

                IsFinePaid = bt.IsFinePaid

            });
        }

        public async Task<MemberResponse?> GetMemberByIdAsync(string membershipId)
        {
            Member? member = await _memberRepository.GetByIdAsync(membershipId);
            if(member == null)
            {
                return null;
            }
            return new MemberResponse
            {
                ApplicationUserId = member.ApplicationUserId,
                Email = member.ApplicationUser.Email!,
                FullName = member.ApplicationUser.FullName,
                MembershipId = member.MembershipId,
                UserName = member.ApplicationUser.UserName!,
                PhoneNumber = member.ApplicationUser.PhoneNumber
            };
        }

       

        public async Task<UpdateMemberResult> UpdateMemberAsync(string membershipId, UpdateMemberRequest request)
        {
            Member? member = await _memberRepository.GetByIdAsync(membershipId);
            if (member == null)
            {
                return new UpdateMemberResult
                {
                    Status = UpdateMemberStatus.NotFound
                };
            }
            ApplicationUser user = member.ApplicationUser; // أنا هيك اخدت الابليكيشن يوزر المرتبط بهاد الميميبر 
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null &&
                existingUser.Id != user.Id)
            {
             return new UpdateMemberResult
             {
                 Status = UpdateMemberStatus.EmailAlreadyExists
             };
            }
            ApplicationUser? existingUserName =
             await _userManager.FindByNameAsync(request.UserName);

            if (existingUserName is not null &&
                existingUserName.Id != user.Id)
            {
                return new UpdateMemberResult
                {
                    Status = UpdateMemberStatus.UserNameAlreadyExists
                };
            }

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.PhoneNumber = request.PhoneNumber;

            IdentityResult updateResult =
                await _userManager.UpdateAsync(user);

           
                if (!updateResult.Succeeded)
                {
                    return new UpdateMemberResult
                    {
                        Status = UpdateMemberStatus.UpdateFailed,
                        Errors = updateResult.Errors
                            .Select(error => error.Description)
                            .ToList()
                    };
                }
            

            return new UpdateMemberResult
            {
                Status = UpdateMemberStatus.Updated,
                UpdatedMember = new MemberResponse
                {
                    MembershipId = member.MembershipId,
                    ApplicationUserId = member.ApplicationUserId,
                    UserName = user.UserName!,
                    FullName = user.FullName,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber
                }
            };
          

        }
    }
}
