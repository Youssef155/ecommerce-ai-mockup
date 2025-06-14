using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Exceptions;
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
            try
            {
                var authResponse = await _authRepository.LoginAsync(model);

                return new Response<AuthResponseDto>(
                    data: authResponse,
                    statusCode: HttpStatusCode.OK,
                    message: "Login successful",
                    isSucceeded: true
                );
            }
            catch (UnAuthorizedException)
            {
                return new Response<AuthResponseDto>(
                    data: null,
                    statusCode: HttpStatusCode.Unauthorized,
                    message: "Unauthorized access",
                    isSucceeded: false
                );
            }

        }



        public async Task<Response<string>> RegisterAsync(UserRegisterDto model)
        {
            var response = await _authRepository.RegisterAsync(model);

            if (response == true)
                return new Response<string>(data: "User successful creation", statusCode: HttpStatusCode.Created, isSucceeded: true);
            return new Response<string>(data: "Creation failed", statusCode: HttpStatusCode.BadRequest, isSucceeded: false);
        }


        public async Task<Response<AuthResponseDto>> RefreshTokenAsync(string email)
        {
            try
            {
                var authResponse = await _authRepository.RefreshTokenAsync(email);

                return new Response<AuthResponseDto>(
                    data: authResponse,
                    statusCode: HttpStatusCode.OK,
                    message: "Token refreshed successfully",
                    isSucceeded: true
                );
            }
            catch (NotFoundException ex)
            {
                return new Response<AuthResponseDto>(
                    data: null,
                    statusCode: HttpStatusCode.NotFound,
                    message: ex.Message,
                    isSucceeded: false
                );
            }
        }

    }
}
