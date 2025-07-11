using AutoMapper;
using ECommerceAIMockUp.Application.DTOs.Category;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.MappingProfiles.CategoryMapping;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
        /* CreateMap<Category, CategoryUpdateDto>();*/
    }
}
