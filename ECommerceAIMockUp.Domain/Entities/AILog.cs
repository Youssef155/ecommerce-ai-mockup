namespace ECommerceAIMockUp.Domain.Entities;

public class AILog
{
    public int Id { get; set; }
    public string RequestedAI { get; set; }
    public string RequestType { get; set; }
    public string PromptText { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
