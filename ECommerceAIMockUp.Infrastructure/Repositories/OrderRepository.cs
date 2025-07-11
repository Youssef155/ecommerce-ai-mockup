using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAIMockUp.Infrastructure.Repositories;
public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync(bool tracking = false)
    {
        IQueryable<Order> query = _context.Orders
            .Include(o => o.OrderItems);

        if (!tracking)
            query = query.AsNoTracking();

        return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
    }

    public async Task<Order?> GetOrderWithDetailsByIdAsync(int id, bool tracking = false)
    {
        var query = _context.Orders.Include(o => o.OrderItems).Where(o => o.Id == id);

        if (!tracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }
}
