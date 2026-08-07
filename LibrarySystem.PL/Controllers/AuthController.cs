using LibrarySystem.BLL.DTOs.Request.Identity;
using LibrarySystem.BLL.DTOs.Response.Identity;
using LibrarySystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto register)
        {
            AuthResponseDto result = await _identityService.RegisterAsync(register);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(
                StatusCodes.Status201Created,
                result);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto login)
        {
            AuthResponseDto result = await _identityService.LoginAsync(login);
            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new
            {
                Message = "You are authenticated."
            });
        }
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new
            {
                Message = "Welcome Admin."
            });
        }

        [HttpGet("member-only")]
        [Authorize(Roles = "Member")]
        public IActionResult MemberOnly()
        {
            return Ok(new
            {
                Message = "Welcome Member."
            });
        }
    }
}
