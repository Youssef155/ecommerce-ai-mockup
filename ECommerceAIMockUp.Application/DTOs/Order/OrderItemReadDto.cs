namespace ECommerceAIMockUp.Application.DTOs.Order;
public class OrderItemReadDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductDetailsId { get; set; }
    public int DesignDetailsId { get; set; }
}
