using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Request.LibraryItem.Magazine
{
    public class AddMagazineRequest
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int YearOfPublication { get; set; } 
        public string IssueNumber { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
