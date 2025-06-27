using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Wrappers;

namespace ECommerceAIMockUp.Application.Services.Interfaces.Services
{
    public interface IProductService
    {
        Task<Response<PaginatedResult<GetAllProductsDto>>> GetAllProductsService(int pageNumber = 1, int pageSize = 10);
    }
}
