using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class DesignDetails : BaseEntity
{
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public float XAxis { get; set; }
    public float YAxis { get; set; }
    public float Opacity { get; set; }
    public float Rotation { get; set; }
    public string Position { get; set; }

    public OrderItem OrderItem { get; set; }

    public int DesignId { get; set; }
    public Design Design { get; set; }
}
