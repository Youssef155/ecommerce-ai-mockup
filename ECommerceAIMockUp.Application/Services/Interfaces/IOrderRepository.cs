using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Services.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetCartAsync(string userId);
        Task<Order> GetWithDetailsAsync(int orderId);
        Task<Order> GetBySessionIdAsync(string sessionId);

    }

}
