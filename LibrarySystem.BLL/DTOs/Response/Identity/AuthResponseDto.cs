using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Identity
{
    public class AuthResponseDto
    {
         public bool IsSuccess { get; set; }

        public string? Message { get; set; } = null;

        public string? UserId { get; set; }

        public string? FullName { get; set; }
        
        public string? UserName { get; set; } = null;

        public string? Email { get; set; }

        public string? Role { get; set; }

        public string? AccessToken { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
