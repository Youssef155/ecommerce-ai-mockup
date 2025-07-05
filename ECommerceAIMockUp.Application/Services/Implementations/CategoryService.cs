using AutoMapper;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Category;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Services.Implementations;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryReadDto>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync(false);
        return _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
    }

    public async Task<CategoryReadDto?> GetByIdAsync(int id)
    {
        var category = await _repo.GetByIdAsunc(id, tracking: false);
        return category is null ? null : _mapper.Map<CategoryReadDto>(category);
    }

    public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
    {
        var entity = _mapper.Map<Category>(dto);
        await _repo.CreateAsync(entity);
        await _repo.SaveAsync();
        return _mapper.Map<CategoryReadDto>(entity);
    }

    public async Task<CategoryReadDto> UpdateAsync(CategoryUpdateDto dto)
    {
        var category = await _repo.GetByIdAsunc(dto.Id, true);
        if (category is null)
            return null;

        _mapper.Map(dto, category);
        _repo.Update(category);
        await _repo.SaveAsync();
        return _mapper.Map<CategoryReadDto>(category);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
        await _repo.SaveAsync();
    }
}
