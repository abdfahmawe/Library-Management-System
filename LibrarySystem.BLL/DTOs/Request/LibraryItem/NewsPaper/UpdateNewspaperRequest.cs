using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Request.LibraryItem.NewsPaper
{
   public class UpdateNewspaperRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Author { get; set; } = null!;

        [Range(1, 9999)]
        public int YearOfPublication { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public string Region { get; set; } = null!;
    }
}
