using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> Product(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productService.GetAllProductsService(pageNumber, pageSize);

            if (result.StatusCode == HttpStatusCode.OK)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("Filter/products")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] string? seasons, [FromQuery] string? gender, [FromQuery] int? categoryId,
                 int pageNumber = 1,
                 int pageSize = 10)
        {
            List<Season> parsedSeasons = new();
            List<Gender>? parsedGender = null;

            if (!string.IsNullOrWhiteSpace(seasons))
            {
                parsedSeasons = seasons
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Select(s => Enum.TryParse<Season>(s, true, out var result) ? (Season?)result : null)
                    .Where(s => s.HasValue)
                    .Select(s => s.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                parsedSeasons = gender
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Select(s => Enum.TryParse<Season>(s, true, out var result) ? (Season?)result : null)
                    .Where(s => s.HasValue)
                    .Select(s => s.Value)
                    .ToList();
            }


            var result = await _productService.GetProductCategoryFilterService(parsedGender, parsedSeasons, categoryId, pageNumber, pageSize);
            return Ok(result);
        }



        [HttpPost("Product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);

            if (result.StatusCode == HttpStatusCode.Created)
                return Ok("Product created successfully");
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Get basic product info and available sizes.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductBasicAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get available colors for a given size.
        /// </summary>
        [HttpGet("{id}/sizes/{size}/colors")]
        public async Task<IActionResult> GetColorsBySize(int id, string size)
        {
            try
            {
                var colors = await _productService.GetAvailableColorsAsync(id, size);
                return Ok(colors);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Product not found.");
            }
            catch (InvalidOperationException)
            {
                return NotFound("No colors found for the specified size.");
            }
        }

        /// <summary>
        /// Get variant details for a specific size and color combination.
        /// </summary>
        [HttpGet("{id}/variant")]
        public async Task<IActionResult> GetVariant(int id, [FromQuery] string size, [FromQuery] string color)
        {
            try
            {
                var variant = await _productService.GetVariantAsync(id, size, color);
                return Ok(variant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Product not found.");
            }
            catch (InvalidOperationException)
            {
                return NotFound("Variant not found for the given size and color.");
            }
        }
    }
}
