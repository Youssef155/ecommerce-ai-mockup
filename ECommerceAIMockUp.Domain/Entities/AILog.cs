namespace ECommerceAIMockUp.Domain.Entities;

public class AILog
{
    public string RequestType { get; set; }
    public string PromptText { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
