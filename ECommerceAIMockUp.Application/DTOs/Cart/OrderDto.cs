using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.DTOs.Cart
{
    public class OrderDto
    {
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal OrderTotal { get; set; }
        public List<OrderItemDto> OrderDetails { get; set; }
    }
}
