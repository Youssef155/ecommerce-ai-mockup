using ECommerceAIMockUp.Application.DTOs.Order;
using ECommerceAIMockUp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers;
[Route("api/[controller]")]
[ApiController]


public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderReadDto>> GetOrderById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid order ID");

        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound($"Order with ID {id} not found");

        return Ok(order);
    }

    [HttpPut("admin/{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderReadDto>> UpdateOrderStatus(int id, [FromBody] OrderUpdateStatusDto dto)
    {
        if (id <= 0)
            return BadRequest("Invalid order ID");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch between route and body");

        var updatedOrder = await _orderService.UpdateOrderStatusAsync(dto);
        if (updatedOrder == null)
            return NotFound($"Order with ID {id} not found");

        return Ok(updatedOrder);
    }

    [HttpPut("admin/{id}/payment-status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderReadDto>> UpdatePaymentStatus(int id, [FromBody] OrderUpdatePaymentDto dto)
    {
        if (id <= 0)
            return BadRequest("Invalid order ID");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch between route and body");

        var updatedOrder = await _orderService.UpdatePaymentStatusAsync(dto);
        if (updatedOrder == null)
            return NotFound($"Order with ID {id} not found");

        return Ok(updatedOrder);
    }

    [HttpDelete("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid order ID");

        var result = await _orderService.DeleteOrderAsync(id);
        if (!result)
            return NotFound($"Order with ID {id} not found");

        return NoContent();
    }
}
