using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.LibraryItem.NewsPaper
{
   public  class NewspaperResponse
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int YearOfPublication { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Region { get; set; } = null!;
    }
}
