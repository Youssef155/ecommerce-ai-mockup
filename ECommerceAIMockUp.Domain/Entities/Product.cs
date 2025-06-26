using ECommerceAIMockUp.Domain.Common;
using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Gender Gender { get; set; }
    public Season Season { get; set; }
    public double Price { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<ProductDetails> ProductDetails { get; set; }
}