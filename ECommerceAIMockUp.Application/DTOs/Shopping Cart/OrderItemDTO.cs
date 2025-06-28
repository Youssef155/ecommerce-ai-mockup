
using System.ComponentModel.DataAnnotations;

namespace ECommerceAIMockUp.Application.DTOs.Shopping_Cart;

public class OrderItemDTO
{
    [Required]
    [Range(1, 100,ErrorMessage ="You must enter a quantity between 1 and 100")]
    public int Quantity { get; set; }
    [Required]
    public int ProductDetailsId { get; set; }
    [Required]
    public int DesignDetailsId { get; set; }
    //public int OrderId { get; set; }
}
