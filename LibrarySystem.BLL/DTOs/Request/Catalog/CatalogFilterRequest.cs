using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Request.Catalog
{
    public class CatalogFilterRequest
    {
        public string? Title { get; set; }

        public string? Author { get; set; }

        public int? Year { get; set; }

        public string? Type { get; set; }
    }
}
