using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Report
{
    public class MostActiveMemberResponse
    {
        public string MembershipId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int BorrowCount { get; set; }

    }
}
