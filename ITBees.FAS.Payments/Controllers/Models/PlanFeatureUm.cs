namespace ITBees.FAS.Payments.Controllers.Models;

public class PlanFeatureUm
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
    public int PlatformFeatureId { get; set; }
    public Guid PlatformSubscriptionPlanGuid { get; set; }
    public int Position { get; set; }
}