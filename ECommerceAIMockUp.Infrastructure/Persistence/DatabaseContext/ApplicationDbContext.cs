using ECommerceAIMockUp.Domain.Common;
using ECommerceAIMockUp.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in base.ChangeTracker.Entries<BaseEntity>()
                .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                item.Entity.UpdatedAt = DateTime.Now;

                if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
