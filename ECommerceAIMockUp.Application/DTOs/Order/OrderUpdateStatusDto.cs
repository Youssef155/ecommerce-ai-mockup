using ECommerceAIMockUp.Domain.Enums;

namespace ECommerceAIMockUp.Application.DTOs.Order;
public class OrderUpdateStatusDto
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
}
