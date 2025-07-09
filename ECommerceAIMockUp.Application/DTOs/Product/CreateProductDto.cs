using ECommerceAIMockUp.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Gender Gender { get; set; }
        public Season Season { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public IFormFile ImgUrl { get; set; }

    }
}
