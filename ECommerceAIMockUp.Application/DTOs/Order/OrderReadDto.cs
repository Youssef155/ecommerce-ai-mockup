using ECommerceAIMockUp.Domain.Enums;

namespace ECommerceAIMockUp.Application.DTOs.Order;
public class OrderReadDto
{
    public int Id { get; set; }
    public string? AppUserId { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public double OrderTotal { get; set; }
    public DateTime? OrderDate { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<OrderItemReadDto> OrderItems { get; set; }
}
