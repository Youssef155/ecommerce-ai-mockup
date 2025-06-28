using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<PaginatedResult<Product>> GetAllProductsAysnc(int pageNumber, int pageSize);
    }
}
