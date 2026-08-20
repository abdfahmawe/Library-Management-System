using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Report
{
    public class MostBorrowedItemResponse
    {
        public string LibraryItemId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int BorrowCount { get; set; }
        public string Type { get; set; } = null!;
    }
}
