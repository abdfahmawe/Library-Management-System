using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
    public class BorrowTransaction : BaseModel
    {
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }  // calulated 

        public DateTime? ReturnDate { get; set; } // nullable and when actualy returned

        public decimal Fine { get; set; }

        public bool IsFinePaid { get; set; }

        // 1 to 1 relationship with Member
        public Member Member { get; set; } = null!; // navigation property
        public string MembershipId { get; set; } = null!; // forgein key
        // 1 to 1 relationship with LibraryItem
        public LibraryItem LibraryItem { get; set; } = null!; // navigation property
        public string LibraryItemId { get; set; } = null!; // forgein key
    }
}
