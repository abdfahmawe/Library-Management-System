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
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
     UserManager<ApplicationUser> userManager,
     ApplicationDbContext dbContext,
     IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _jwtSettings = jwtOptions.Value;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.Email);
            if(existingUser is null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }
            bool isPasswordRight = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if(!isPasswordRight)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
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

                await _dbContext.Members.AddAsync(member);
                await _dbContext.SaveChangesAsync();
                IList<string> roles =
    await _userManager.GetRolesAsync(user);

                JwtTokenResult jwt =
                    GenerateJwtToken(user, roles);

                await transaction.CommitAsync();

                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Registration successful.",
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = roles.FirstOrDefault(),
                    AccessToken = jwt.AccessToken,
                    ExpiresAt = jwt.ExpiresAt
                };

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private JwtTokenResult GenerateJwtToken(ApplicationUser user,IList<string> roles)
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


    }
}
