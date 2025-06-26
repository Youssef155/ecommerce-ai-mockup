using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public int ProductDetailsId { get; set; }
    public ProductDetails ProductDetails { get; set; }
    
    public int DesignDetailsId { get; set; }
    public DesignDetails DesignDetails { get; set; }
}
