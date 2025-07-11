﻿using System.Linq.Expressions;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(bool tracking, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsunc(int id, bool tracking, params Expression<Func<T, object>>[] includes);
        Task<T> CreateAsync(T entity);
        Task<T> Update(T entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        IQueryable<T> GetAllQueryable(bool tracking, params Expression<Func<T, object>>[] includes);
        Task<int> SaveAsync();
    }
}
