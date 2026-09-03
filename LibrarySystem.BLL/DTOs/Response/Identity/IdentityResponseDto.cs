using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Identity
{
    public class IdentityResponseDto
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }
    }
}
