using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Services.Interfaces.Authentication;
using ECommerceAIMockUp.Application.Wrappers;
using System.Net;
namespace ECommerceAIMockUp.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto model)
        {
            var response = await _authRepository.LoginAsync(model);
            return response;
        }

        public async Task<Response<string>> RegisterAsync(UserRegisterDto model)
        {
            var response = await _authRepository.RegisterAsync(model);

            if (response == true)
                return new Response<string>(data: "User successful creation", statusCode: HttpStatusCode.Created, isSucceeded: true);
            return new Response<string>(data: "Creation failed", statusCode: HttpStatusCode.BadRequest, isSucceeded: false);
        }
    }
}
