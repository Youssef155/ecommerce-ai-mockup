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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="user")]
    public class ShoppingCartController(ICartService cartService,UserManager<AppUser> userManager) : ControllerBase
    {
        private string? UserId
        {
            get
            {
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Id == null)
                {
                    return null;
                }
                return Id;
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAll()
        {
            if (UserId == null)
            {
                return Unauthorized();
            }
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Unauthorized();
            }
            var items = await cartService.GetAllItems(user.Id);
            return Ok(items);
        }

        [HttpGet("{ItemId}")]
        public async Task<ActionResult<OrderItem>> GetById([FromRoute] int ItemId)
        {
            if (UserId == null)
            {
                return Unauthorized();
            }
            var item = await cartService.GetItemById(UserId,ItemId);
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] OrderItemDTO orderItem)
        {
            //var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null)
            {
                return Unauthorized();
            }
            await cartService.AddItem(orderItem,UserId);
            return CreatedAtAction(nameof(GetAll),null);
        }

        [HttpDelete("{ItemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int ItemId)
        {
            if (UserId == null)
                return Unauthorized();
            var item = await cartService.GetItemById(UserId, ItemId);
            await cartService.Remove(UserId, item);
            return CreatedAtAction(nameof(GetAll), null);
        }
    }
}
