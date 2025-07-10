
using ECommerceAIMockUp.Application.DTOs.Shopping_Cart;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Services.Interfaces.Cart_Service;

public interface ICartRepository
{
    Task<IEnumerable<OrderItemDTO>> GetAllItems(string UserId);
    Task<OrderItem> GetItemById(string Id, int orderId);
    Task AddItem (OrderDTO item,string userId);
    Task Remove(string userId, int orderId, int productId, int designId);
    Task UpdateQuantity(string userId,int ItemId, int quantity);
    Task AddOrderItem(OrderItemDTO item);

}
