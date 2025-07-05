using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerceAIMockUp.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        private IQueryable<T> AddIncludes(Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _context.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool tracking, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            if (includes != null && includes.Any())
                query = AddIncludes(includes);

            return await query.ToListAsync();
        }

        public IQueryable<T> GetAllQueryable(bool tracking, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            if (includes != null && includes.Any())
                query = AddIncludes(includes);

            return query;
        }

        public async Task<T> GetByIdAsunc(int id, bool tracking, params Expression<Func<T, object>>[] includes)
        {
            if (id <= 0)
                return null;

            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            if (includes != null && includes.Any())
                query = AddIncludes(includes);

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
    }
}
