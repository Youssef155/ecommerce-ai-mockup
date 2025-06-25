using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(oi => oi.productDetails)
                .WithOne()
                .HasForeignKey<OrderItem>(oi => oi.ProductDetailsId);

            builder.HasOne(oi => oi.DesignDetails)
                .WithOne()
                .HasForeignKey<OrderItem>(oi => oi.DesignDetailsId);
        }
    }
}
