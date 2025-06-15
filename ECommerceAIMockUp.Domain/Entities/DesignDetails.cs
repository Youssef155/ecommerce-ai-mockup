using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class DesignDetails : BaseEntity
{
    public float Scale { get; set; }
    public float XAxis { get; set; }
    public float YAxis { get; set; }
    public string Position { get; set; }

    public int DesignId { get; set; }
    public Design Design { get; set; }

    public int OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; }
}
