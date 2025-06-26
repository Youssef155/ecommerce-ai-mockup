using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AILogConfiguration : IEntityTypeConfiguration<AILog>
{
    public void Configure(EntityTypeBuilder<AILog> builder)
    {

        builder.HasOne(a => a.Design)
            .WithOne(a => a.AILog)
            .HasForeignKey<AILog>(a => a.DesignId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
