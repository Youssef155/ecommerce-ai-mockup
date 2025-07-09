using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.Enums;
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
            var products = GetAllQueryable(tracking: false, includes: new Expression<Func<Product, object>>[] { p => p.Category, p => p.ProductDetails });

            var OrderedProducts = products.OrderBy(p => p.CreatedAt);

            var paginatedResult = await OrderedProducts.ToPaginatedListAsync(pageNumber, pageSize);

            return paginatedResult;
        }

        public async Task<PaginatedResult<Product>> GetFilterProductCategory(List<Gender>? gender, List<Season>? season, int? categoryId, int pageNumber, int pageSize)
        {
            var products = GetAllQueryable(tracking: false, includes: new Expression<Func<Product, object>>[] { p => p.Category, p => p.ProductDetails });


            if (gender != null && gender.Any())
            {
                products = products.Where(p => gender.Contains(p.Gender));
            }

            if (season != null && season.Any())
            {
                products = products.Where(p => season.Contains(p.Season));
            }


            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);

            }

            var paginatedResult = await products.ToPaginatedListAsync(pageNumber, pageSize);

            return paginatedResult;
        }


        public async Task<Product> CreateProductManuallyAsync(Product product, ProductDetails details)
        {
            product.ProductDetails = new List<ProductDetails> { details };
            await CreateAsync(product);
            return product;
        }

        public async Task<Product?> GetByIdWithVariantsAsync(int productId)
        {
            return await _products
                .Include(p => p.ProductDetails)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
}
