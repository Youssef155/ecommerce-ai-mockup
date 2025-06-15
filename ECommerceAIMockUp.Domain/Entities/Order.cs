using ECommerceAIMockUp.Domain.Common;
using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Domain.Entities;

public class Order : BaseEntity
{
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public string Status { get; set; }

    public string PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }

    public double OrderTotal { get; set; }
    public DateTime? OrderDate { get; set; }
    
    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }

    public string PhoneNumber { get; set; }
    public Address ShippingAddress { get; set; }

}