using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        builder.HasData(
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
