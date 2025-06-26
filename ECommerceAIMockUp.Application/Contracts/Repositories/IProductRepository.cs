using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<List<Product>> GetProductAysnc();
    }
}
