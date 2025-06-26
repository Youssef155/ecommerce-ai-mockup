using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, sa =>
            {
                sa.Property(a => a.Street).HasMaxLength(100);
                sa.Property(a => a.City).HasMaxLength(50);
                sa.Property(a => a.Governorate).HasMaxLength(50);
                sa.Property(a => a.Zip).HasMaxLength(10);
            });

            builder.HasMany(o => o.OrderItems)
               .WithOne(oi => oi.Order)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}