using AutoMapper;
using ECommerceAIMockUp.Application.DTOs.user;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAIMockUp.Application.MappingProfiles.UserMapping;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<IdentityUser, UserReadDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}
