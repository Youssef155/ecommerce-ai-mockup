namespace ECommerceAIMockUp.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; }
        public string Token { get; set; } = default!;
    }
}
