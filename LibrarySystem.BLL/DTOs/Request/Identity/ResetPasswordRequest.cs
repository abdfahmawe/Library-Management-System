using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Request.Identity
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
