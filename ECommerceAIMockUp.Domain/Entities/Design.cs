using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class Design : BaseEntity
{
    public string ImageUrl { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public AILog AILog { get; set; }

    public ICollection<DesignDetails> DesignDetails { get; set; }
}
