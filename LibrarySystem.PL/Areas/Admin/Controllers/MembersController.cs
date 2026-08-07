using LibrarySystem.BLL.Common.Enums;
using LibrarySystem.BLL.DTOs.Request.Member;
using LibrarySystem.BLL.DTOs.Response.Member;
using LibrarySystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllMembers()
        {
            IEnumerable<MemberResponse> members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }
        [HttpGet("{membershipId}")]
        public async Task<ActionResult<MemberResponse>> GetMemberByMembershipId([FromRoute]string membershipId)
        {
            MemberResponse? member = await _memberService.GetMemberByIdAsync(membershipId);
            if (member is null)
            {
                return NotFound(new
                {
                    Message = "Member was not found."
                });
            }
            return Ok(member);
            
        }

        [HttpPut("{membershipId}")]
        public async Task<ActionResult<MemberResponse>> UpdateMember([FromRoute] string membershipId, [FromBody] UpdateMemberRequest request)
        {
            UpdateMemberResult updatedMember = await _memberService.UpdateMemberAsync(membershipId, request);
           switch(updatedMember.Status)
            {
                case UpdateMemberStatus.Updated:
                    return Ok(updatedMember.UpdatedMember);
                case UpdateMemberStatus.NotFound:
                    return NotFound(new { Message = "Member was not found." });
                case UpdateMemberStatus.EmailAlreadyExists:
                    return Conflict(new { Message = "Email already exists." });
                case UpdateMemberStatus.UserNameAlreadyExists:
                    return Conflict(new { Message = "Username already exists." });
                case UpdateMemberStatus.UpdateFailed:
                    return StatusCode(500, new { Message = "Failed to update member.", Errors = updatedMember.Errors });
                default:
                    return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{membershipId}")]
        public async Task<IActionResult> DeleteMember([FromRoute] string membershipId)
        {
            var result = await _memberService.DeleteMemberAsync(membershipId);
            return result switch
            {
                DeleteMemberResult.Deleted => Ok(new { Message = "Member deleted successfully." }),
                DeleteMemberResult.NotFound => NotFound(new { Message = "Member was not found." }),
                DeleteMemberResult.HasBorrowingHistory => Conflict(new { Message = "Cannot delete member with borrowing history." }),
                DeleteMemberResult.DeleteFailed => StatusCode(500, new { Message = "Failed to delete member." }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred." })
            };
        }

        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] AddMemberRequest request)
        {
            var result = await _memberService.AddMemberAsync(request);
           
            switch (result.Status)
            {
                case AddMemberStatus.Created:

                    return CreatedAtAction(
                        nameof(GetMemberByMembershipId),
                        new
                        {
                            membershipId = result.Member!.MembershipId
                        },
                        result.Member);

                case AddMemberStatus.EmailAlreadyExists:

                    return Conflict(new
                    {
                        Message = "Email already exists."
                    });

                case AddMemberStatus.UserNameAlreadyExists:

                    return Conflict(new
                    {
                        Message = "Username already exists."
                    });

                case AddMemberStatus.CreationFailed:

                    return StatusCode(500, new
                    {
                        Message = "Failed to create member.",
                        Errors = result.Errors
                    });

                default:

                    return StatusCode(500, new
                    {
                        Message = "Unexpected error."
                    });
            }


        }

        [HttpGet("{membershipId}/borrowings")]
        public async Task<ActionResult<IEnumerable<MemberBorrowingResponse>>> GetMemberBorrowings([FromRoute] string membershipId)
        {
            var borrowings = await _memberService.GetMemberBorrowingsAsync(membershipId);
            if (borrowings is null)
            {
                return NotFound(new
                {
                    Message = "Member was not found."
                });
            }
            return Ok(borrowings);
        }
    }
}
