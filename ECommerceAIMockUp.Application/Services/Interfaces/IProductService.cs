using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.ValueObjects;
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
        Task<ProductVariantDto> GetVariantAsync(int productId, string size, string color);
        Task<Response<PaginatedResult<GetAllProductsDto>>> GetAllProductsService(int pageNumber = 1, int pageSize = 10);

        Task<Response<PaginatedResult<GetAllProductsDto>>> GetProductCategoryFilterService(List<Gender>? gender, List<Season>? seasons, int? categoryId, int pageNumber = 1, int pageSize = 10);

        Task<Response<string>> CreateProductAsync(CreateProductDto product);
    }

}
