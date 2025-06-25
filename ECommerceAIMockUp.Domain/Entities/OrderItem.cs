using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    // No table referencing
    public int ProductDetailsId { get; set; }

    // naming the variable Exact to it's type
    //public ProductDetails ProductDetails { get; set; }
    public ProductDetails productDetails { get; set; }

    // no table referencing
    public int DesignDetailsId { get; set; }
    public DesignDetails DesignDetails { get; set; }
}
