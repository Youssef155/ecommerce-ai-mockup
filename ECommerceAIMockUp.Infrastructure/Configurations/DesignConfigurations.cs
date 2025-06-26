using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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
