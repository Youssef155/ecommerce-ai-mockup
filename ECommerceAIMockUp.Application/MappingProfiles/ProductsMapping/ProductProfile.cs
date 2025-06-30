using AutoMapper;
using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.MappingProfiles.ProductsMapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetAllProductsDto>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));


            //CreateMap<CreateProductDto, Product>()
            //        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
            //            (Gender)Enum.Parse(typeof(Gender), src.Gender, true)))
            //        .ForMember(dest => dest.Season, opt => opt.MapFrom(src =>
            //            (Season)Enum.Parse(typeof(Season), src.Season, true)))
            //        .ForMember(dest => dest.ProductDetails, opt => opt.Ignore())
            //        .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            //        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            //        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));


        }

    }
}
