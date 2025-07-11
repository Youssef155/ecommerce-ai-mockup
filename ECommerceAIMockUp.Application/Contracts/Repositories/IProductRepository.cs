using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Domain.Enums;
using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<PaginatedResult<Product>> GetAllProductsAysnc(int pageNumber, int pageSize);

        public Task<PaginatedResult<Product>> GetFilterProductCategory(List<Gender>? gender, List<Season>? season, int? categoryId, int pageNumber, int pageSize);

        Task<Product> CreateProductManuallyAsync(Product product, ProductDetails details);
        Task<Product?> GetByIdWithVariantsAsync(int productId);
    }
}
