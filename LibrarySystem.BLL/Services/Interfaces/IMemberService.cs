using LibrarySystem.BLL.Common.Enums;
using LibrarySystem.BLL.DTOs.Request.Member;
using LibrarySystem.BLL.DTOs.Response.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberResponse>> GetAllMembersAsync();
        Task<MemberResponse?> GetMemberByIdAsync(string membershipId);
        // update 
        Task<UpdateMemberResult> UpdateMemberAsync(string membershipId, UpdateMemberRequest request);
        Task<DeleteMemberResult> DeleteMemberAsync(string membershipId);
       
        Task<AddMemberResult> AddMemberAsync(AddMemberRequest request);
        Task<IEnumerable<MemberBorrowingResponse>?> GetMemberBorrowingsAsync(string membershipId);
    }
}
