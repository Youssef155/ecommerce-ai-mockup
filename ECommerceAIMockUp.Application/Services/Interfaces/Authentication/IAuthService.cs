using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Wrappers;

namespace ECommerceAIMockUp.Application.Services.Interfaces.Authentication
{
    public interface IAuthService
    {
        public Task<Response<AuthResponseDto>> LoginAsync(LoginDto model);

        public Task<Response<string>> RegisterAsync(UserRegisterDto model);

        public Task<Response<AuthResponseDto>> RefreshTokenAsync(string Email);
    }
}
