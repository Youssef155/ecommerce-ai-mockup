using ECommerceAIMockUp.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductBasicDto> GetProductBasicAsync(int productId);
        Task<ColorOptionsDto> GetAvailableColorsAsync(int productId, string size);
        Task<ProductVariantDto> GetVariantAsync(Guid productId, string size, string color);
    }

}
