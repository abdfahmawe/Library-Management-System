using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
   public class Magazine : LibraryItem
    {
        public string IssueNumber { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
