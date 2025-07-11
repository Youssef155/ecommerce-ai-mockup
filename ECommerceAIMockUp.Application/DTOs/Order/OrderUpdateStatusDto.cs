using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Application.DTOs.Order;
public class OrderUpdateStatusDto
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
}
