using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerceAIMockUp.Infrastructure.Persistence.Contracts.Authentication
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(IJwtTokenGenerator jwtTokenGenerator,
            SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user is null /* ||  user.EmailConfirmed == false */)
                return new Response<AuthResponseDto>(data: null, statusCode: HttpStatusCode.Unauthorized,
                    message: "Invalid user or password"
                    , isSucceeded: false);

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return new Response<AuthResponseDto>(data: null, statusCode: HttpStatusCode.Unauthorized,
                    "Invalid user or password",
                    isSucceeded: false);

            var TokenData = new JwtUserData
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id
            };

            var Token = _jwtTokenGenerator.CreateJwt(TokenData);

            var response = new AuthResponseDto { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Token = Token };

            return new Response<AuthResponseDto>(response, HttpStatusCode.OK, "Login Successful", isSucceeded: true);
        }

        public async Task<bool> RegisterAsync(UserRegisterDto model)
        {
            if (await CheckEmailExistsAsync(model.Email))
                return false;

            var newUser = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                City = model.City,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            return true;
        }


        public async Task<AuthResponseDto> RefreshTokenAsync(string userEmail)
        {
            var currentUser = await _userManager.FindByEmailAsync(userEmail);

            var Tokendata = await MapToJwtUserData(currentUser!);

            var TokenGenerated = _jwtTokenGenerator.CreateJwt(Tokendata);

            var response = new AuthResponseDto()
            {
                FirstName = Tokendata.FirstName,
                LastName = Tokendata.LastName,
                Email = Tokendata.Email,
                Token = TokenGenerated
            };

            return response;
        }


        #region Helping Methods

        public AuthResponseDto Authenticate(AppUser user)
        {
            var jwtUserData = new JwtUserData
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var tokenGenerated = _jwtTokenGenerator.CreateJwt(jwtUserData);

            return new AuthResponseDto { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Token = tokenGenerated };
        }

        private async Task<bool> CheckEmailExistsAsync(string Email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == Email.ToLower());
        }

        private async Task<JwtUserData> MapToJwtUserData(AppUser user)
        {
            return new JwtUserData
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id
            };
        }

        #endregion
    }
}
