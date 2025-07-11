using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceAIMockUp.Application.DTOs.Cart
{
    public class AddToCartDto
    {
        public int ProductDetailsId { get; set; }
        public int DesignDetailsId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
