using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAIMockUp.Infrastructure.Configurations;

public static class CategorySeedings
{
    public static void CreateCategories(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Hoodie", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) },
                new Category { Name = "T-Shirt", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) },
                new Category { Name = "Shoes", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) },
                new Category { Name = "Jacket", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) },
                new Category { Name = "Jeans", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) }

            );

            context.SaveChanges();
        }
    }
}
