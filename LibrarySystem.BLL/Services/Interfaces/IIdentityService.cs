using LibrarySystem.BLL.DTOs.Request.Identity;
using LibrarySystem.BLL.DTOs.Response.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Interfaces
{
   public interface IIdentityService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> ConfirmEmailAsync(string userId, string token);

        // Forget + Reset Password 

        Task<IdentityResponseDto> ForgetPasswordAsync(ForgotPasswordRequest request);
        Task<IdentityResponseDto> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
