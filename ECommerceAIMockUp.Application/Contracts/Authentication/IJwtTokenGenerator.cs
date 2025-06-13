using ECommerceAIMockUp.Application.DTOs.Auth;

namespace ECommerceAIMockUp.Application.Contracts.Authentication
{
    public interface IJwtTokenGenerator
    {
        public string CreateJwt(JwtUserData user);
    }
}
