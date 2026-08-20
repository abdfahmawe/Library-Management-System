using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Report
{
    public class FinesOverTimeResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalFines { get; set; }
    }
}
