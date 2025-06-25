using ECommerceAIMockUp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Address)
                .HasMaxLength(250);

            builder.Property(u => u.City)
                .HasMaxLength(100);

            builder.HasMany(u => u.Designs)
                .WithOne(d => d.AppUser)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.AppUser)
                .HasForeignKey(o => o.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AILogs)
                .WithOne(l => l.AppUser)
                .HasForeignKey(l => l.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
