using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
    public class Member
    {
        public string MembershipId { get; set; } = Guid.NewGuid().ToString();

        // 1 to 1 relationship with ApplicationUser
        public ApplicationUser ApplicationUser { get; set; } = null!; // navigation property
        public string ApplicationUserId { get; set; } = null!; // forgein key

        // 1 to many relationship with BorrowTransaction
        public ICollection<BorrowTransaction> BorrowTransactions { get; set; } = new List<BorrowTransaction>(); // navigation property
       
    }
}
