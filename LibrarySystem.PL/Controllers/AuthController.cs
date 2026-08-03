using LibrarySystem.BLL.DTOs.Request.Identity;
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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto register)
        {
            var result = await _identityService.RegisterAsync(register);
            return Ok(new
            {
                Message = result
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            var result = await _identityService.LoginAsync(login);
            return Ok(new
            {
                Message = result
            });
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
