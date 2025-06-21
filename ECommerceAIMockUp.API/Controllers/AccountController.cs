using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            var response = await _authService.RegisterAsync(model);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var response = await _authService.LoginAsync(model);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return Unauthorized(response);
        }


        [HttpGet("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                var response = await _authService.RefreshTokenAsync(userEmail);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(response);
                }

                return Unauthorized(response);
            }
            return BadRequest("Invalid Token");
        }
    }
}
