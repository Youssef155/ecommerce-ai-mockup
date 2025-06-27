namespace ECommerceAIMockUp.Application.DTOs.Product;

public class ProductVariantDto
{
    public Guid ProductId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}