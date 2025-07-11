using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<int?> GetByNameAsync(string CategoryName);
    }
}
