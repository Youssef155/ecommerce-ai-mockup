using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class Design : BaseEntity
{
    public string ImageUrl { get; set; }

    // Not identifying the table referencing
    // Appuser is Identity Soooooooo it is string
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public ICollection<DesignDetails> DesignDetails { get; set; }
}
