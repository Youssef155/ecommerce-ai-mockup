using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.ValueObjects;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Order> GetCartAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.ProductDetails)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.DesignDetails)
                .FirstOrDefaultAsync(o => o.AppUserId == userId && o.Status == OrderStatus.CartItem);
        }

        public async Task<Order> GetWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.ProductDetails)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.DesignDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.ProductDetails)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.DesignDetails)
                .FirstOrDefaultAsync(o => o.SessionId == sessionId);
        }

    }
}
