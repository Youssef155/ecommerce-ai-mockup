using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using ECommerceAIMockUp.Infrastructure.Extensions;
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

        public async Task<PaginatedResult<Product>> GetAllProductsAysnc(int pageNumber, int pageSize)
        {
            var products = GetAllQueryable(tracking: false, includes: new Expression<Func<Product, object>>[] { p => p.Category });

            var OrderedProducts = products.OrderBy(p => p.CreatedAt);

            var paginatedResult = await OrderedProducts.ToPaginatedListAsync(pageNumber, pageSize);

            return paginatedResult;
        }
    }
}
