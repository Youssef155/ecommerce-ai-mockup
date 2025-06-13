using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            var response = await _authService.RegisterAsync(model);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var response = await _authService.LoginAsync(model);

            return StatusCode((int)response.StatusCode, response);
        }


    }
}
