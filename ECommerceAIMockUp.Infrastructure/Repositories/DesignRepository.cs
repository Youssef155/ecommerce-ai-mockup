using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.Repositories
{
    public class DesignRepository : BaseRepository<Design>, IDesignRepository
    {
        private readonly ApplicationDbContext _context;

        public DesignRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Design>> GetAllDesignByUserIdAsync(string userId)
        {
            return await _context.Designs.AsNoTracking().Where(d => d.AppUserId ==  userId).OrderByDescending(d => d.CreatedAt).ToListAsync();
        }
    }
}
