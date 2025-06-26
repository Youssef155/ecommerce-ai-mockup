using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerceAIMockUp.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly DbSet<Product> _products;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _products = db.Set<Product>();
        }

        public async Task<List<Product>> GetProductAysnc()
        {
            var products = await GetAllAsync(
                tracking: false,
                includes: new Expression<Func<Product, object>>[]
                {
                  p => p.Category
                });

            return products.ToList();
        }

    }
}
