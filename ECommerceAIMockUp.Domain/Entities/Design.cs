using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class Design : BaseEntity
{
    public string ImageUrl { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public ICollection<DesignDetails> DesignDetails { get; set; }
}
