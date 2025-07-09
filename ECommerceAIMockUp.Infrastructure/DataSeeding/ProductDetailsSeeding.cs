using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.Enums;
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
            var tshirtProduct = context.Products.FirstOrDefault(c => c.Gender == Gender.Unisex);
            var hoodieProduct = context.Products.FirstOrDefault(c => c.Gender == Gender.Male);

            context.ProductDetails.AddRange( 
                new ProductDetails
                {
                    Color = "White",
                    Size = "M",
                    ImgUrl = "img1.png",
                    Amount = 50,
                    ProductId = tshirtProduct.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                },
                new ProductDetails
                {
                    Color = "Black",
                    Size = "L",
                    ImgUrl = "img2.png",
                    Amount = 30,
                    ProductId = tshirtProduct.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                },
                new ProductDetails
                {
                    Color = "Gray",
                    Size = "XL",
                    ImgUrl = "img3.png",
                    Amount = 20,
                    ProductId = tshirtProduct.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8)
                }
            );

            context.SaveChanges();
        }
    }
}
