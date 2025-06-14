using ECommerceAIMockUp.Application.DTOs.Auth;

namespace ECommerceAIMockUp.Application.Contracts.Authentication
{
    public interface IAuthRepository
    {
        public Task<AuthResponseDto> LoginAsync(LoginDto model);

        public Task<bool> RegisterAsync(UserRegisterDto model);

        public Task<AuthResponseDto> RefreshTokenAsync(string userEmail);
    }
}
