﻿using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.DTOs.Product;

public class ProductBasicDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImgUrl { get; set; }
    public List<string> AvailableSizes { get; set; }
}