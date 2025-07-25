﻿using AutoMapper;
using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.MappingProfiles.ProductsMapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetAllProductsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ProductDetails
                    .Select(pd => pd.ImgUrl).FirstOrDefault()));

        }
    }
}
