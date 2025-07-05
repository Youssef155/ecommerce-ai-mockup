using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Application.DTOs.Order;
public class OrderUpdatePaymentDto
{
    public int Id { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
}
