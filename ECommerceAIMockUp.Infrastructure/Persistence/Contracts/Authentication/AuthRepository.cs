using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.DTOs.Auth;
using ECommerceAIMockUp.Application.Exceptions;
using ECommerceAIMockUp.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user is null)
                throw new UnAuthorizedException();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                throw new UnAuthorizedException();

            return Authenticate(user);
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

            if (currentUser is null)
                throw new NotFoundException(nameof(AppUser), userEmail);

            return Authenticate(currentUser);
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
