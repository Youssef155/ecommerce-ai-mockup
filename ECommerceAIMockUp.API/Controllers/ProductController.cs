using ECommerceAIMockUp.Application.Services.Interfaces.Services;
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
    }
}
