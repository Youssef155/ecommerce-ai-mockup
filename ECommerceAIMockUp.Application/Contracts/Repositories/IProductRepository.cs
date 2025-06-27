using ECommerceAIMockUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdWithVariantsAsync(int productId);
    }
}
