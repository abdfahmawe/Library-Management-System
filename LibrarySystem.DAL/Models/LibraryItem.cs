using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
  
   public abstract class LibraryItem : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int YearOfPublication { get; set; }
        public bool IsAvailable { get; set; } = true;

        // 1 to many relationship with BorrowTransaction

        public ICollection<BorrowTransaction> BorrowTransactions { get; set; } = new List<BorrowTransaction>(); // navigation property


    }
}
