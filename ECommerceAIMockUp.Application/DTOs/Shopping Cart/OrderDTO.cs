﻿
using System.ComponentModel.DataAnnotations;

namespace ECommerceAIMockUp.Application.DTOs.Shopping_Cart;

public class OrderDTO
{

    //[Required]
    //public int ProductDetailsId { get; set; }
    //[Required]
    //public int DesignDetailsId { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }

    public string PhoneNumber { get; set; } = "TEMP";
    public string Street { get; set; } = "TEMP";
    public string City { get; set; } = "TEMP";
    public string Governorate { get; set; } = "TEMP";
    public string Zip { get; set; } = "TEMP";

}
