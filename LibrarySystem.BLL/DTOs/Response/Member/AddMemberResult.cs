using LibrarySystem.BLL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Member
{
   public class AddMemberResult
    {
        public AddMemberStatus Status { get; set; }
        public MemberResponse? Member { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
