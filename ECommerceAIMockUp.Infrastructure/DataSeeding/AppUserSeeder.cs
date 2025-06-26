using ECommerceAIMockUp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public static class AppUserSeeder
    {        
        public static async Task SeedAppUserAsync(this IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string role = "Customer";
            string email = "omar@gmail.com";

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                 user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = false,
                    FirstName = "Omar",
                    LastName = "Mehanna",
                    Address = "Camp shezar",
                    City = "Alexandria"
                };

                var result = await userManager.CreateAsync(user, "Omar@123");
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(user, "Customer");
            }
            
        }
    }
}
