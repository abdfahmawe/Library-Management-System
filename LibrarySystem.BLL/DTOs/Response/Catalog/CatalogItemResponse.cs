using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Catalog
{
    public class CatalogItemResponse
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int YearOfPublication { get; set; }

        public string Type { get; set; } = null!;

        public bool IsAvailable { get; set; }
    }
}
