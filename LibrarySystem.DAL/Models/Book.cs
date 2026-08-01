using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
   public class Book : LibraryItem
    {
        
        public string ISBN { get; set; } = null!;
        public int NumberOfPages { get; set; }
    }
}
