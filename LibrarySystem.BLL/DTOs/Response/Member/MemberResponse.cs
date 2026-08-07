using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Member
{
    public class MemberResponse
    {
        public string MembershipId { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!;
        public string UserName { get; set; } = null!;


        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }
    }
}
