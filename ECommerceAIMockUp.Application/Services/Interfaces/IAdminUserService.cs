using ECommerceAIMockUp.Application.DTOs.user;

namespace ECommerceAIMockUp.Application.Services.Interfaces;
public interface IAdminUserService
{
    Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
    Task<bool> PromoteToAdminAsync(string userId);
}
