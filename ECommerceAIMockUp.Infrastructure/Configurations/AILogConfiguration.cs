using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AILogConfiguration : IEntityTypeConfiguration<AILog>
{
    public void Configure(EntityTypeBuilder<AILog> builder)
    {
        builder.HasOne(a => a.AppUser)
            .WithMany(u => u.AILogs)
            .HasForeignKey(a => a.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Design)
            .WithMany()
            .HasForeignKey(a => a.DesignId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
