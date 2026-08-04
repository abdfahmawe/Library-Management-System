using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Request.LibraryItem.Book
{
    public class UpdateBookRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Author { get; set; } = null!;

        [Range(1, 9999)]
        public int YearOfPublication { get; set; }

        [Range(1, int.MaxValue)]
        public int NumberOfPages { get; set; }

    }
}
