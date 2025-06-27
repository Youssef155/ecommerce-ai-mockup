using ECommerceAIMockUp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Product()
        {
            return Ok(new { message = "You can view our products." });
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
