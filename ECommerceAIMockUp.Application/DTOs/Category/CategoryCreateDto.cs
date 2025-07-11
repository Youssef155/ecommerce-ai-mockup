using System.ComponentModel.DataAnnotations;

namespace ECommerceAIMockUp.Application.DTOs.Category;
public class CategoryCreateDto
{
    [Required]
    public string Name { get; set; } = "";
}
