using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdWithVariantsAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.ProductDetails)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

    }
}
