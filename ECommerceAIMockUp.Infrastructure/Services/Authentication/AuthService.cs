using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Services.Interfaces.Authentication;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerceAIMockUp.Infrastructure.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(
            IJwtTokenGenerator jwtTokenGenerator,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return new Response<AuthResponseDto>(
                    data: null,
                    statusCode: HttpStatusCode.Unauthorized,
                    isSucceeded: false
                );
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return new Response<AuthResponseDto>(
                    data: null,
                    statusCode: HttpStatusCode.Unauthorized,
                    isSucceeded: false
                );
            }

            var response = Authenticate(user);

            return new Response<AuthResponseDto>(
                data: response,
                statusCode: HttpStatusCode.OK,
                isSucceeded: true
            );
        }

        public async Task<Response<string>> RegisterAsync(UserRegisterDto model)
        {
            if (await CheckEmailExistsAsync(model.Email))
            {
                return new Response<string>(
                    data: "Email already exists.",
                    statusCode: HttpStatusCode.Conflict,
                    isSucceeded: false
                );
            }

            var newUser = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                Address = model.Street
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                return new Response<string>(
                    data: "User successfully created.",
                    statusCode: HttpStatusCode.Created,
                    isSucceeded: true
                );
            }

            var errors = string.Join("; ", result.Errors.Select(e => e.Description));

            return new Response<string>(
                data: "User creation failed.",
                statusCode: HttpStatusCode.BadRequest,
                isSucceeded: false
            );
        }

        public async Task<Response<AuthResponseDto>> RefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new Response<AuthResponseDto>(
                    data: null,
                    statusCode: HttpStatusCode.NotFound,
                    isSucceeded: false
                );
            }

            var response = Authenticate(user);

            return new Response<AuthResponseDto>(
                data: response,
                statusCode: HttpStatusCode.OK,
                isSucceeded: true
            );
        }

        public async Task<Response<string>> LogoutAsync()
        {
            return new Response<string>(
                data: "User successfully logged out.",
                statusCode: HttpStatusCode.OK,
                isSucceeded: true
            );
        }


        #region Helper

        private AuthResponseDto Authenticate(AppUser user)
        {
            var jwtUserData = new JwtUserData
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var token = _jwtTokenGenerator.CreateJwt(jwtUserData);

            return new AuthResponseDto { Token = token };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

        #endregion
    }
}
