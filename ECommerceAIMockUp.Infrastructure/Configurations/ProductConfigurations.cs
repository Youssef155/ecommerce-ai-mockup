using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.HasMany(p => p.ProductDetails)
                .WithOne(pd => pd.Product)
                .HasForeignKey(pd => pd.ProductId);
        }
    }
}