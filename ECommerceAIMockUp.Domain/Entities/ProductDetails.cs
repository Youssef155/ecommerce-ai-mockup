using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class ProductDetails : BaseEntity
{
    public string Color { get; set; }
    public string Size { get; set; }
    public int Amount { get; set; }
    public string ImgUrl { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public ICollection<OrderItem> OrderItem { get; set; }
}
