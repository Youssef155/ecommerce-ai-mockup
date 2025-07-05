namespace ECommerceAIMockUp.Application.DTOs.user;
public class UserReadDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();

}
