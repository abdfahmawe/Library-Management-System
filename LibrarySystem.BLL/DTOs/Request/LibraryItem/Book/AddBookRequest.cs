using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Request.LibraryItem.Book
{
   public class AddBookRequest
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int YearOfPublication { get; set; }

        public string ISBN { get; set; } = null!;

        public int NumberOfPages { get; set; }
    }
}
