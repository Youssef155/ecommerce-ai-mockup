using AutoMapper;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Services.Interfaces.Caching;
using ECommerceAIMockUp.Application.Services.Interfaces.FileServices;
using ECommerceAIMockUp.Application.Services.Interfaces.Services;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using System.Net;

namespace ECommerceAIMockUp.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _Mapper;
        private readonly IRedisService _redisService;
        private readonly IFileService _fileservice;
        private readonly ICategoryRepository _categoryRespository;

        public ProductService(IProductRepository productRepository, IMapper Mapper, IRedisService redisService, IFileService fileservice,
            ICategoryRepository categoryRespository)
        {
            _productRepository = productRepository;
            _Mapper = Mapper;
            _redisService = redisService;
            _fileservice = fileservice;
            _categoryRespository = categoryRespository;
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

        public async Task<Response<PaginatedResult<GetAllProductsDto>>> GetProductCategoryFilterService(int categoryId, int pageNumber = 1, int pageSize = 10)
        {
            var cachedKey = $"product_page: {pageNumber} page_size: {pageSize} categoryid: {categoryId}";
            var cachedData = _redisService.GetData<PaginatedResult<GetAllProductsDto>>(cachedKey);

            if (cachedData != null)
                return new Response<PaginatedResult<GetAllProductsDto>>(data: cachedData, HttpStatusCode.OK, isSucceeded: true);

            var productCategory = await _productRepository.GetFilterProductCategory(categoryId, pageNumber, pageSize);

            var mappedData = _Mapper.Map<List<GetAllProductsDto>>(productCategory.Data);

            var paginatedDto = PaginatedResult<GetAllProductsDto>.Success(mappedData, productCategory.TotalCount,
                productCategory.CurrentPage, productCategory.PageSize);

            var response = new Response<PaginatedResult<GetAllProductsDto>>(paginatedDto, HttpStatusCode.OK, true);

            return response;
        }

        public async Task<Response<string>> CreateProductAsync(CreateProductDto dto)
        {
            try
            {
                var categoryId = await _categoryRespository.GetByNameAsync(dto.CategoryName);
                var fileName = await _fileservice.SaveFile(dto.ImgUrl, "uploads/products", ".jpg,.jpeg,.png,.webp");

                var productDetails = new ProductDetails
                {
                    Color = dto.Color,
                    Size = dto.Size,
                    Amount = dto.Amount,
                    ImgUrl = $"/uploads/products/{fileName}"
                };

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Gender = dto.Gender,
                    Season = dto.Season,
                    Price = dto.Price,
                    CategoryId = categoryId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _productRepository.CreateProductManuallyAsync(product, productDetails);

                return new Response<string>("Product created successfully", HttpStatusCode.Created, true);
            }
            catch (Exception ex)
            {
                return new Response<string>($"Failed to create product: {ex.Message}", HttpStatusCode.InternalServerError, false);
            }
        }

    }
}
