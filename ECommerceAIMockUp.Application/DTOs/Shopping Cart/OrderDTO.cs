
using System.ComponentModel.DataAnnotations;

namespace ECommerceAIMockUp.Application.DTOs.Shopping_Cart;

public class OrderDTO
{
    [Required]
    [Range(1, 100,ErrorMessage ="You must enter a quantity between 1 and 100")]
    public int Quantity { get; set; }
    [Required]
    public int ProductDetailsId { get; set; }
    [Required]
    public int DesignDetailsId { get; set; }


    public string PhoneNumber { get; set; }

    public string Street { get; set; }
    public string City { get; set; }
    public string Governorate { get; set; }
    public string Zip { get; set; }
    //public int OrderId { get; set; }
}
