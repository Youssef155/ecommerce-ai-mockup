using ECommerceAIMockUp.Application.DTOs.Shopping_Cart;
using ECommerceAIMockUp.Application.Services.Interfaces.Cart_Service;
using ECommerceAIMockUp.Domain;
using ECommerceAIMockUp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/{UserId}/[controller]")]
    [ApiController]
    [Authorize(Roles ="user")]
    public class ShoppingCartController(ICartService cartService,UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAll([FromRoute] string UserId)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Unauthorized();
            }
            var items = await cartService.GetAllItems(user.Id);
            return Ok(items);
        }
        [HttpGet]
        public async Task<ActionResult<OrderItem>> GetById([FromRoute] string UserId, [FromRoute] int ItemId)
        {
            var item = await cartService.GetItemById(UserId,ItemId);
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] OrderItemDTO orderItem)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null)
            {
                return Unauthorized();
            }
            await cartService.AddItem(orderItem,UserId);
            return Created();
        }
    }
}
