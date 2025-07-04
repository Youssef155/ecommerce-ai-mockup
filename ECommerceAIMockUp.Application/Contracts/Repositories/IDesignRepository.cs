using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IDesignRepository : IBaseRepository<Design>
    {
        Task<IEnumerable<Design>> GetAllDesignByUserIdAsync(string userId);
    }
}
