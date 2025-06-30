using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IBaseRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAllAsync(bool tracking, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsunc(int id, bool tracking, params Expression<Func<T, object>>[] includes);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
