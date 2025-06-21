using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        [HttpGet("Products")]
        public IActionResult Product()
        {
            return Ok(new JsonResult(new { Message = "Only authorized users can view players." }));
        }
    }
}
