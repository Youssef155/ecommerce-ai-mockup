using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.DTOs.Cart
{
    public class OrderItemDto
    {
        public int ProductDetailsId { get; set; }
        public int DesignDetailsId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}
