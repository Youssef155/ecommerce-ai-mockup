using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Wrappers;

namespace ECommerceAIMockUp.Application.Contracts.Authentication
{
    public interface IAuthRepository
    {
        public Task<Response<AuthResponseDto>> LoginAsync(LoginDto model);

        public Task<bool> RegisterAsync(UserRegisterDto model);

        public Task<AuthResponseDto> RefreshTokenAsync(string userEmail);
    }
}
