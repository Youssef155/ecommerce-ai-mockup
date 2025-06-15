using ECommerceAIMockUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAIMockUp.Domain;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? City { get; set; }
    public string? Address { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<AILog> AILogs { get; set; }
}
