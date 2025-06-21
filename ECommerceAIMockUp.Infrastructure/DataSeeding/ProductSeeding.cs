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

        if (!context.Products.Any())
        {

            var tshirtCategory = context.Categories.FirstOrDefault(c => c.Name == "T-Shirt");
            var hoodieCategory = context.Categories.FirstOrDefault(c => c.Name == "Hoodie");

            context.Products.AddRange(
                new Product
                {
                    Name = "Classic White T-Shirt",
                    Description = "A timeless classic for all genders.",
                    Gender = "Unisex",
                    Season = "All Seasons",
                    Price = 29.99,
                    CategoryId = tshirtCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                },
                new Product
                {
                    Name = "Hoodie",
                    Description = "Stay warm during the coldest days.",
                    Gender = "Male",
                    Season = "Winter",
                    Price = 39.99,
                    CategoryId = hoodieCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                }
            );

            context.SaveChanges();
        }
    }
}
