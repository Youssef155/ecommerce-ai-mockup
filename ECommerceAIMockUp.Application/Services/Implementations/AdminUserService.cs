using AutoMapper;
using ECommerceAIMockUp.Application.DTOs.user;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Domain;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAIMockUp.Application.Services.Implementations;
public class AdminUserService : IAdminUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public AdminUserService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();

        var result = new List<UserReadDto>();

        foreach (var user in users)
        {
            var dto = _mapper.Map<UserReadDto>(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            result.Add(dto);
        }

        return result;
    }
    public async Task<bool> PromoteToAdminAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return false;

        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        return true;
    }
}
