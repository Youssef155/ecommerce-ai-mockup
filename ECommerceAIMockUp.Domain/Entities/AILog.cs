namespace ECommerceAIMockUp.Domain.Entities;

public class AILog
{
    // No Primary key
    public int Id { get; set; }
    public string RequestType { get; set; }
    public string PromptText { get; set; }
    public bool IsSuccesed { get; set; }

    // Not identifying the table referencing
    public int DesignId { get; set; }
    public Design Design { get; set; }

    // Not identifying the table referencing
    // Appuser is Identity sooooooo it's id is string
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
