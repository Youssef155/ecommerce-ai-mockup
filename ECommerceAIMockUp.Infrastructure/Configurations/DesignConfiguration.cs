using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class DesignConfiguration : IEntityTypeConfiguration<Design>
    {
        public void Configure(EntityTypeBuilder<Design> builder)
        {
            builder.HasMany(d => d.DesignDetails)
                .WithOne(d => d.Design)
                .HasForeignKey(d => d.DesignId);
        }
    }
}
