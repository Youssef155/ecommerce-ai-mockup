using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Application.Services.Interfaces.Services
{
    public interface IProductService
    {
        Task<Response<PaginatedResult<GetAllProductsDto>>> GetAllProductsService(int pageNumber = 1, int pageSize = 10);

        Task<Response<PaginatedResult<GetAllProductsDto>>> GetProductCategoryFilterService(List<Gender>? gender, List<Season>? seasons, int? categoryId, int pageNumber = 1, int pageSize = 10);

        Task<Response<string>> CreateProductAsync(CreateProductDto product);
    }
}
