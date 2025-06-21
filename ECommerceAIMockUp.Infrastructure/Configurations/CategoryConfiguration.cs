using ECommerceAIMockUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = 1,
                    CreatedAt = new DateTime(2025, 5, 21),
                    UpdatedAt = new DateTime(2025, 6, 19),
                    Name = "Hoodie"
                },

                new Category
                {
                    Id = 2,
                    CreatedAt = new DateTime(2025, 5, 21),
                    UpdatedAt = new DateTime(2025, 6, 19),
                    Name = "T-Shirt"
                }
             );
        }
    }
}
