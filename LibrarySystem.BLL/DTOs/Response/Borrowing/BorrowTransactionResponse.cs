using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Borrowing
{
    public class BorrowTransactionResponse
    {
        public string Id { get; set; } = null!;

        public string LibraryItemId { get; set; } = null!;

        public string LibraryItemTitle { get; set; } = null!;

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
