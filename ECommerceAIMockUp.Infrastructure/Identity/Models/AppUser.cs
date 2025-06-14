using Microsoft.AspNetCore.Identity;

namespace ECommerceAIMockUp.Infrastructure.Identity.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string? City { get; set; }

        public string? Address { get; set; }
    }
}
