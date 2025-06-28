
using ECommerceAIMockUp.Application.DTOs.Shopping_Cart;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Services.Interfaces.Cart_Service;

public interface ICartService
{
    Task<IEnumerable<OrderItem>> GetAllItems(string UserId);
    Task<OrderItem> GetItemById(string Id, int orderId);
    Task AddItem (OrderItemDTO item,string userId);
    Task Remove (string userId, OrderItem orderItem);
    Task RemoveRange (string userId, List<OrderItem> items);
}
