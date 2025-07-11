using ECommerceAIMockUp.Application.DTOs.Category;

namespace ECommerceAIMockUp.Application.Services.Interfaces;
public interface ICategoryService
{
    Task<IEnumerable<CategoryReadDto>> GetAllAsync();
    Task<CategoryReadDto?> GetByIdAsync(int id);
    Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryReadDto> UpdateAsync(CategoryUpdateDto dto);
    Task DeleteAsync(int id);
}
