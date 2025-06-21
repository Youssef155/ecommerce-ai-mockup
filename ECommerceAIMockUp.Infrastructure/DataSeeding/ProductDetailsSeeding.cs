using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public static class ProductDetailsSeeding
{
    public static void CreateProductDetails(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.ProductDetails.Any())
        {
            context.ProductDetails.AddRange(
                new ProductDetails
                {
                    Id = 1,
                    Color = "White",
                    Size = "M",
                    Amount = 50,
                    ProductId = 1,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                },
                new ProductDetails
                {
                    Id = 2,
                    Color = "Black",
                    Size = "L",
                    Amount = 30,
                    ProductId = 1,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                },
                new ProductDetails
                {
                    Id = 3,
                    Color = "Gray",
                    Size = "XL",
                    Amount = 20,
                    ProductId = 2,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                }
            );

            context.SaveChanges();
        }
    }
}
