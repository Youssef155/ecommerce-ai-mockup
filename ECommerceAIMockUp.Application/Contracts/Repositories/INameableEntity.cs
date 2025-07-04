namespace ECommerceAIMockUp.Application.Contracts.Repositories
{
    public interface INameableEntity
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
