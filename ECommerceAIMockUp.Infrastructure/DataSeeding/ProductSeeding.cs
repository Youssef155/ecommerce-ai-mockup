using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.ValueObjects;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.Extensions.DependencyInjection;



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
            var shoesCategory = context.Categories.FirstOrDefault(c => c.Name == "Shoes");
            var jeansCategory = context.Categories.FirstOrDefault(c => c.Name == "Jeans");
            var jacketsCategory = context.Categories.FirstOrDefault(c => c.Name == "Jacket");

            context.Products.AddRange(
                new Product
                {
                    Name = "Classic White T-Shirt",
                    Description = "A timeless classic for all genders.",
                    Gender = Gender.Unisex,
                    Season = Season.Summer,
                    Price = 29.99,
                    CategoryId = tshirtCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails{Color = "white", Size = "M", Amount = 50, ImgUrl = "/uploads/products/white-tshirt-m.jpg" }
                    }
                },
                new Product
                {
                    Name = "Hoodie",
                    Description = "Stay warm during the coldest days.",
                    Gender = Gender.Male,
                    Season = Season.Winter,
                    Price = 39.99,
                    CategoryId = hoodieCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails { Color = "Black", Size = "M", Amount = 25, ImgUrl = "/uploads/products/black-hoodie.jpg" }
                    }
                },
                new Product
                {
                    Name = "Classic White T-Shirt",
                    Description = "A timeless classic for all genders.",
                    Gender = Gender.Unisex,
                    Season = Season.Summer,
                    Price = 29.99,
                    CategoryId = tshirtCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails { Color = "White", Size = "M", Amount = 50, ImgUrl = "/uploads/products/white-tshirt-m.jpg" },
                    }
                },
                new Product
                {
                    Name = "Men's Hoodie",
                    Description = "Stay warm during the coldest days.",
                    Gender = Gender.Male,
                    Season = Season.Winter,
                    Price = 39.99,
                    CategoryId = hoodieCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails { Color = "Black", Size = "M", Amount = 25, ImgUrl = "/uploads/products/black-hoodie.jpg" }
                    }
                },
                new Product
                {
                    Name = "Denim Jacket",
                    Description = "A versatile jacket for cool weather.",
                    Gender = Gender.Female,
                    Season = Season.Autumn,
                    Price = 59.99,
                    CategoryId = jacketsCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails { Color = "Blue", Size = "S", Amount = 20, ImgUrl = "/uploads/products/denim-jacket-s.jpg" },
                    }
                },
                new Product
                {
                    Name = "Slim Fit Jeans",
                    Description = "Comfortable slim-fit jeans.",
                    Gender = Gender.Male,
                    Season = Season.Spring,
                    Price = 49.99,
                    CategoryId = jeansCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails { Color = "Dark Blue", Size = "34", Amount = 35, ImgUrl = "/uploads/products/jeans-34.jpg" }
                    }
                },
                new Product
                {
                    Name = "Unisex Sneakers",
                    Description = "Stylish sneakers for daily wear.",
                    Gender = Gender.Unisex,
                    Season = Season.Spring,
                    Price = 69.99,
                    CategoryId = shoesCategory.Id,
                    CreatedAt = new DateTime(2025, 6, 1),
                    UpdatedAt = new DateTime(2025, 6, 8),
                    ProductDetails = new List<ProductDetails>
                    {
                        new ProductDetails { Color = "White", Size = "9", Amount = 45, ImgUrl = "/uploads/products/sneakers-9.jpg"},
                    }
                }
            );

            context.SaveChanges();
        }
    }
}
