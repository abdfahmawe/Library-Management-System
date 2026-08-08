using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.LibraryItem
{
    public class LibraryItemDetailsResponse
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int YearOfPublication { get; set; }
        public bool IsAvailable { get; set; }
        public string Type { get; set; } = null!;

        // Book
        public string? ISBN { get; set; }
        public int? NumberOfPages { get; set; }

        // Magazine
        public string? IssueNumber { get; set; }
        public string? Category { get; set; }

        // Newspaper
        public DateTime? PublicationDate { get; set; }
        public string? Region { get; set; }
    }
}
