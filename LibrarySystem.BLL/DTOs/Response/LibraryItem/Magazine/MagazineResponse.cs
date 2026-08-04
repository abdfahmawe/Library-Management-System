using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.LibraryItem.Magazine
{
    public class MagazineResponse
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int YearOfPublication { get; set; }

        public bool IsAvailable { get; set; }
        public string IssueNumber { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
