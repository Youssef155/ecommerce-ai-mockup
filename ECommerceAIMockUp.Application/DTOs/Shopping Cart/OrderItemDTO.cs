using ECommerceAIMockUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.DTOs.Shopping_Cart
{
    public class OrderItemDTO
    {
        [Required]
        [Range(1, 100, ErrorMessage = "You must enter a quantity between 1 and 100")]
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductDetailsId { get; set; }
        public int DesignDetailsId { get; set; }

        public double OrderTotal { get; set; }
    }
}
