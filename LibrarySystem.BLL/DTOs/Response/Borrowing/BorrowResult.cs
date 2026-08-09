using LibrarySystem.BLL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Borrowing
{
    public class BorrowResult
    {
        public BorrowStatus Status { get; set; }

        public BorrowTransactionResponse? BorrowTransaction { get; set; }
    }
}
