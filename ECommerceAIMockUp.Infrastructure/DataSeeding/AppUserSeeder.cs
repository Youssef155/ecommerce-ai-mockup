using ECommerceAIMockUp.Domain;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public static class AppUserSeeder
    {
        public static async Task SeedAppUserAsync(UserManager<AppUser> userManager)
        {
            var user = new AppUser
            {
                UserName = "omar@gmail.com",
                Email = "omar@gmail.com",
                EmailConfirmed = true,
                FirstName = "Omar",
                LastName = "Mehanna"
            };

            var result = await userManager.CreateAsync(user, "Omar@123");
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, "Customer");

        }
    }
}
