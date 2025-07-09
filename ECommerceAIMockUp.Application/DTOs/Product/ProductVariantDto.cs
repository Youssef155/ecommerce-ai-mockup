namespace ECommerceAIMockUp.Application.DTOs.Product;

public class ProductVariantDto
{
    public int ProductId { get; set; }
    public int ProductDetailsId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}