using System.Security.Claims;
using ECommerceAIMockUp.Application.Cases.CartCases;
using ECommerceAIMockUp.Application.DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartCase _cartCase;

        public CartController(CartCase cartCase)
        {
            _cartCase = cartCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var items = await _cartCase.GetCartItemsAsync(userId);
            return Ok(items);
        }

        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseQuantity([FromBody] int productDetailsId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            await _cartCase.IncreaseQuantityAsync(userId, productDetailsId);
            return NoContent();
        }

        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseQuantity([FromBody] int productDetailsId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            await _cartCase.DecreaseQuantityAsync(userId, productDetailsId);
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var url = await _cartCase.SubmitOrderAsync(userId);
            return Ok(new { redirectUrl = url });
        }

        [HttpPost("confirm")] // triggered from webhook or client after success
        public async Task<IActionResult> ConfirmOrder([FromBody] string sessionId)
        {
            await _cartCase.ConfirmOrderAsync(sessionId);
            return Ok();
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            await _cartCase.AddToCartAsync(userId, dto.ProductDetailsId, dto.DesignDetailsId, dto.Quantity);
            return Ok();
        }
    }
}
