using ECommerceAIMockUp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IAdminUserService _adminUserService;

    public UsersController(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetAll()
    {
        var users = await _adminUserService.GetAllUsersAsync();
        return Ok(users);
    }
    [HttpPut("admin/promote/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> PromoteToAdmin(string id)
    {
        var result = await _adminUserService.PromoteToAdminAsync(id);
        return result ? NoContent() : NotFound("User not found");
    }


}
