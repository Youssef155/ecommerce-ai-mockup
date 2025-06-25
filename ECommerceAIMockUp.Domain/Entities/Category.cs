using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }

    // No Collection for products as One to Many
    public ICollection<Product> products { get; set; }

}
