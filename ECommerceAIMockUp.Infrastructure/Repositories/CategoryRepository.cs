using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _category;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _category = db.Set<Category>();
        }

        public async Task<int> GetByNameAsync(string CategoryName)
        {
            var CategorySelect = await _category.FirstOrDefaultAsync(c => c.Name.ToLower() == CategoryName.ToLower());
            return CategorySelect.Id;
        }
    }
}
