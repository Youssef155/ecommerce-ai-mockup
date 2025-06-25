using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Category)
                .WithMany(c => c.products)
                .HasForeignKey(p => p.CategoryId);

            builder.HasMany(p => p.productDetails)
                .WithOne(pd => pd.Product)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
