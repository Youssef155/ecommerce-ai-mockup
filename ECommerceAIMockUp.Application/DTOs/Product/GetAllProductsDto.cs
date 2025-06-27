using ECommerceAIMockUp.Domain.ValueObjects;

namespace ECommerceAIMockUp.Application.DTOs.Product
{
    public class GetAllProductsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Gender Gender { get; set; }
        public Season Season { get; set; }
        public double Price { get; set; }

        public string CategoryName { get; set; }
    }
}
