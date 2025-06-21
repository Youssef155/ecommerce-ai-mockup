using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Infrastructure.Configurations;

public static class CategorySeedings
{
    public static void CreateCategories(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Id = 1, Name = "Hoodie", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) },
                new Category { Id = 2, Name = "T-Shirt", CreatedAt = new DateTime(2025, 3, 1), UpdatedAt = new DateTime(2025, 5, 1) }
            );
        }
    }
}
