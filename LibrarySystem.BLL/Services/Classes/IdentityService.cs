using LibrarySystem.BLL.DTOs.Request.Identity;
using LibrarySystem.BLL.DTOs.Response.Identity;
using LibrarySystem.BLL.Services.Interfaces;
using LibrarySystem.BLL.Setting;
using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibrarySystem.BLL.Services.Classes
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
     UserManager<ApplicationUser> userManager,
     ApplicationDbContext dbContext,
     IOptions<JwtSettings> jwtOptions,
     IEmailSender emailSender,
     IOptions<EmailSettings> emailSettings)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _emailSender = emailSender;
            _emailSettings = emailSettings.Value;
            _jwtSettings = jwtOptions.Value;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }
            bool isPasswordRight = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if (!isPasswordRight)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }
            if (!existingUser.EmailConfirmed)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Please confirm your email before logging in.",
                };
            }
            IList<string> roles =
    await _userManager.GetRolesAsync(existingUser);

            JwtTokenResult jwt =
                GenerateJwtToken(existingUser, roles);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful.",
                UserId = existingUser.Id,
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                UserName = existingUser.UserName,
                Role = roles.FirstOrDefault(),
                AccessToken = jwt.AccessToken,
                ExpiresAt = jwt.ExpiresAt
            };

        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            ApplicationUser? existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Email is already registered."
                };
            }
            ApplicationUser? existingUserName =
                await _userManager.FindByNameAsync(request.UserName);

            if (existingUserName is not null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Username is already taken."
                };
            }
            await using var transaction =
                await _dbContext.Database.BeginTransactionAsync();

            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    UserName = request.UserName,

                    PhoneNumber = request.PhoneNumber
                };

                IdentityResult createResult =
                    await _userManager.CreateAsync(user, request.Password);

                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = string.Join(", ",
                        createResult.Errors.Select(
                       error => error.Description))
                    };
                }

                IdentityResult roleResult =
                    await _userManager.AddToRoleAsync(user, "Member");

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = string.Join(", ",
                        roleResult.Errors.Select(
                      error => error.Description))
                    };
                }

                Member member = new Member
                {

                    ApplicationUserId = user.Id
                };
                //
                string tokenForEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string cofirmationLink = $"{_emailSettings.ConfirmationUrl}/api/Auth/confirm-email" +
                                         $"?userId={Uri.EscapeDataString(user.Id)}" +
                                         $"&token={Uri.EscapeDataString(tokenForEmail)}";
                string emailBody = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset=""UTF-8"">
                            <title>Confirm Your Email</title>
                        </head>
                        <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;"">

                            <div style=""max-width: 600px; margin: auto; background-color: white; padding: 30px; border-radius: 10px;"">

                                <h2 style=""text-align: center;"">Welcome to Library System 📚</h2>

                                <p>Hello <strong>{user.FullName}</strong>,</p>

                                <p>
                                    Thank you for registering in our Library System.
                                    Please confirm your email address to activate your account.
                                </p>

                                <div style=""text-align: center; margin: 30px 0;"">
                                    <a href=""{cofirmationLink}""
                                       style=""background-color: #007bff;
                                              color: white;
                                              padding: 12px 25px;
                                              text-decoration: none;
                                              border-radius: 6px;
                                              display: inline-block;"">
                                        Confirm Email
                                    </a>
                                </div>

                                <p>
                                    After confirming your email, you will be able to log in to your account.
                                </p>

                                <p style=""color: #777; font-size: 13px;"">
                                    If you did not create this account, you can safely ignore this email.
                                </p>

                                <hr>

                                <p style=""text-align: center; color: #999; font-size: 12px;"">
                                    © 2026 Library System. All rights reserved.
                                </p>

                            </div>

                        </body>
                        </html>";

                await _emailSender.SendEmailAsync(user.Email!, "Confirm Email ", emailBody);


                //
                await _dbContext.Members.AddAsync(member);
                await _dbContext.SaveChangesAsync();

                //            IList<string> roles =
                //await _userManager.GetRolesAsync(user);

                //            JwtTokenResult jwt =
                //                GenerateJwtToken(user, roles);

                await transaction.CommitAsync();

                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Registration successful. Please check your email to confirm your account.",
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,

                };

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private JwtTokenResult GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
              {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("username", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
              };
            foreach (string role in roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role));
            }
            SymmetricSecurityKey securityKey =
             new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

            SigningCredentials signingCredentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            DateTime expiresAt =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings.DurationInMinutes);

            JwtSecurityToken jwtToken =
                new JwtSecurityToken(
           issuer: _jwtSettings.Issuer,
           audience: _jwtSettings.Audience,
           claims: claims,
           expires: expiresAt,
           signingCredentials: signingCredentials);

            string accessToken =
            new JwtSecurityTokenHandler()
           .WriteToken(jwtToken);

            return new JwtTokenResult
            {
                AccessToken = accessToken,
                ExpiresAt = expiresAt
            };
        }
        public async Task<AuthResponseDto> ConfirmEmailAsync(string userId, string token)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }
            if (user.EmailConfirmed)
            {
                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Email is already confirmed."
                };
            }
            IdentityResult result =
                await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description))
                };
            }
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Email confirmed successfully."
            };
        }

        public async Task<IdentityResponseDto> ForgetPasswordAsync(ForgotPasswordRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new IdentityResponseDto
                {
                    IsSuccess = false,
                    Message = "If this email exists, a password reset link will be sent."
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
         

            await _emailSender.SendEmailAsync(user.Email!, "Your password reset token is:", token);
            return new IdentityResponseDto
            {
                IsSuccess = true,
                Message = "If this email exists, a password reset link has been sent."
            };
        }

        public async Task<IdentityResponseDto> ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new IdentityResponseDto
                {
                    IsSuccess = false,
                    Message = "Passwords do not match."
                };
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new IdentityResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid password reset request."
                };
            }
            // we will not check if the token is valid here, because the UserManager will handle that for us.
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if(!result.Succeeded)
            {
                return new IdentityResponseDto
                {
                    IsSuccess = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            return new IdentityResponseDto
            {
                IsSuccess = true,
                Message = "Password has been reset successfully."
            };
        }
    }
}
