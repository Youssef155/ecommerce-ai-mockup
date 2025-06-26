using ECommerceAIMockUp.Domain.Common;

namespace ECommerceAIMockUp.Domain.Entities;

public class AILog : BaseEntity
{
    public string RequestType { get; set; }
    public string PromptText { get; set; }
    public bool IsSuccesed { get; set; }

    public int? DesignId { get; set; }
    public Design? Design { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
