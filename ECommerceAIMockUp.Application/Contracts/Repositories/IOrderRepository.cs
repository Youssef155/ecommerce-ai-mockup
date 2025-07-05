using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Contracts.Repositories;
public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersWithDetailsAsync(bool tracking = false);
    Task<Order?> GetOrderWithDetailsByIdAsync(int id, bool tracking = false);
}
