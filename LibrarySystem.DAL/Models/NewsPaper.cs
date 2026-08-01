using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
   public class Newspaper : LibraryItem
    {
        public DateTime PublicationDate { get; set; }
        public string Region { get; set; } = null!;
    }
}
