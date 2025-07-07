using AutoMapper;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Services.Interfaces.Caching;
using ECommerceAIMockUp.Application.Services.Interfaces.FileServices;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.ValueObjects;
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
        public async Task<ProductBasicDto> GetProductBasicAsync(int productId)
        {
            var product = await _productRepository.GetByIdWithVariantsAsync(productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var sizes = product.ProductDetails
                .Select(v => v.Size)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            return new ProductBasicDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                AvailableSizes = sizes
            };
        }

        public async Task<Response<PaginatedResult<GetAllProductsDto>>> GetProductCategoryFilterService(List<Gender>? gender, List<Season>? seasons, int? categoryId, int pageNumber = 1, int pageSize = 10)
        {
            var genderKey = (gender != null && gender.Any()) ? string.Join("-", gender.Select(s => s.ToString().ToLower())) : "any";

            var seasonsKey = (seasons != null && seasons.Any())
                ? string.Join("_", seasons.Select(s => s.ToString().ToLower()))
                : "any";

            var categoryKey = categoryId.HasValue ? categoryId.Value.ToString() : "any";


            var cachedKey = $"product_category:{categoryKey} product_gender:{genderKey} product_season:{seasonsKey} product_page:{pageNumber} page_size:{pageSize}";
            var cachedData = _redisService.GetData<PaginatedResult<GetAllProductsDto>>(cachedKey);

            if (cachedData != null)
                return new Response<PaginatedResult<GetAllProductsDto>>(data: cachedData, HttpStatusCode.OK, isSucceeded: true);

            var filteredProducts = await _productRepository.GetFilterProductCategory(gender, seasons, categoryId, pageNumber, pageSize);

            var mappedData = _Mapper.Map<List<GetAllProductsDto>>(filteredProducts.Data);

            var paginatedDto = PaginatedResult<GetAllProductsDto>.Success(mappedData, filteredProducts.TotalCount,
                filteredProducts.CurrentPage, filteredProducts.PageSize);

            var response = new Response<PaginatedResult<GetAllProductsDto>>(paginatedDto, HttpStatusCode.OK, true);

            return response;
        }

        public async Task<ColorOptionsDto> GetAvailableColorsAsync(int productId, string size)
        {
            var product = await _productRepository.GetByIdWithVariantsAsync(productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var colors = product.ProductDetails
                .Where(v => v.Size.Equals(size, StringComparison.OrdinalIgnoreCase))
                .Select(v => v.Color)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            if (!colors.Any())
                throw new InvalidOperationException("No colors found for the specified size.");

            return new ColorOptionsDto
            {
                Size = size,
                AvailableColors = colors
            };
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

        public async Task<ProductVariantDto> GetVariantAsync(int productId, string size, string color)
        {
            var product = await _productRepository.GetByIdWithVariantsAsync(productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var variant = product.ProductDetails
                .FirstOrDefault(v =>
                    v.Size.Equals(size, StringComparison.OrdinalIgnoreCase) &&
                    v.Color.Equals(color, StringComparison.OrdinalIgnoreCase));

            if (variant == null)
                throw new InvalidOperationException("Variant not found for the given size and color.");

            return new ProductVariantDto
            {
                ProductId = product.Id,
                Size = variant.Size,
                Color = variant.Color,
                Price = variant.Product.Price,
                Stock = variant.Amount
            };
        }

    }
}
