namespace ECommerceAIMockUp.Application.DTOs.Auth
{
    public class JwtUserData
    {
        public string UserId { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
