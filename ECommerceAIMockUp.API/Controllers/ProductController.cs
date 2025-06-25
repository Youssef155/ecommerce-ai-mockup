using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        [HttpGet("products")]
        public IActionResult Product()
        {
            return Ok(new { message = "You can view our products." });
        }
    }
}
