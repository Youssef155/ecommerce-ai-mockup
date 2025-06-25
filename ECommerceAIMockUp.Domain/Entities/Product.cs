using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }
    public double Price { get; set; }

    // no table referencing
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    // Naming the same type
    public ICollection<ProductDetails> productDetails { get; set; }
}