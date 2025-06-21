using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;



public static class ProductSeeding
{
    public static void CreateProducts(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Categories.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Id = 1,
                    Name = "Classic White Shirt",
                    Description = "A timeless classic for all genders.",
                    Gender = "Unisex",
                    Season = "All Seasons",
                    Price = 29.99,
                    CategoryId = 2,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                },
                new Product
                {
                    Id = 2,
                    Name = "Hoodie",
                    Description = "Stay warm during the coldest days.",
                    Gender = "Male",
                    Season = "Winter",
                    Price = 39.99,
                    CategoryId = 1,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                }
            );
        }
    }
}
