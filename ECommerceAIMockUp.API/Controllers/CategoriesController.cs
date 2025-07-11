using ECommerceAIMockUp.Application.DTOs.Category;
using ECommerceAIMockUp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<CategoryReadDto>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid category ID");

        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
            return NotFound($"Category with ID {id} not found");

        return Ok(category);
    }

    [HttpPost]

    public async Task<ActionResult<CategoryReadDto>> Create([FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCategory = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id}")]

    public async Task<ActionResult<CategoryReadDto>> Update(int id, [FromBody] CategoryUpdateDto dto)
    {
        if (id <= 0)
            return BadRequest("Invalid category ID");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch between route and body");

        var updatedCategory = await _categoryService.UpdateAsync(dto);
        return Ok(updatedCategory);
    }

    [HttpDelete("{id}")]

    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid category ID");

        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}
