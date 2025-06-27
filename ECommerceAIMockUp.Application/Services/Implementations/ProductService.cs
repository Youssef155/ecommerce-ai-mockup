using AutoMapper;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Services.Interfaces.Caching;
using ECommerceAIMockUp.Application.Services.Interfaces.Services;
using ECommerceAIMockUp.Application.Wrappers;
using System.Net;

namespace ECommerceAIMockUp.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _Mapper;
        private readonly IRedisService _redisService;

        public ProductService(IProductRepository productRepository, IMapper Mapper, IRedisService redisService)
        {
            _productRepository = productRepository;
            _Mapper = Mapper;
            _redisService = redisService;
        }

        public async Task<Response<PaginatedResult<GetAllProductsDto>>> GetAllProductsService(int pageNumber = 1, int pageSize = 10)
        {
            var cachedKey = $"product_page: {pageNumber} product_size: {pageSize}";

            var cachedData = _redisService.GetData<PaginatedResult<GetAllProductsDto>>(cachedKey);

            if (cachedData != null)
                return new Response<PaginatedResult<GetAllProductsDto>>(data: cachedData, HttpStatusCode.OK, isSucceeded: true);


            var productResult = await _productRepository.GetAllProductsAysnc(pageNumber, pageSize);

            var mappedData = _Mapper.Map<List<GetAllProductsDto>>(productResult.Data);

            var paginatedDto = PaginatedResult<GetAllProductsDto>.Success(mappedData, productResult.TotalCount,
                productResult.CurrentPage, productResult.PageSize);

            var response = new Response<PaginatedResult<GetAllProductsDto>>(paginatedDto, HttpStatusCode.OK, true);

            return response;
        }
    }
}
