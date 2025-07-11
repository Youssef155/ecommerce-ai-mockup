using ECommerceAIMockUp.Application.DTOs.Order;

namespace ECommerceAIMockUp.Application.Services.Interfaces;
public interface IOrderService
{
    Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync();
    Task<OrderReadDto?> GetOrderByIdAsync(int id);
    Task<OrderReadDto?> UpdateOrderStatusAsync(OrderUpdateStatusDto dto);
    Task<OrderReadDto?> UpdatePaymentStatusAsync(OrderUpdatePaymentDto dto);
    Task<bool> DeleteOrderAsync(int id);
}
