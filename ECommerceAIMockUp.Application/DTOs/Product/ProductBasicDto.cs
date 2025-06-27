namespace ECommerceAIMockUp.Application.DTOs.Product;

public class ProductBasicDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> AvailableSizes { get; set; }
}