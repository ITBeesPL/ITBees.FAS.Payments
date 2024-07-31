namespace ITBees.FAS.Payments.Interfaces.Models;

public class PlanFeature
{
    public int Id { get; set; }
    public PlatformSubscriptionPlan PlatformSubscriptionPlan { get; set; }
    public Guid PlatformSubscriptionPlanGuid { get; set; }
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public string? Description { get; set; }
    public int Position { get; set; }
    public PlatformFeature PlatformFeature { get; set; }
    public int PlatformFeatureId { get; set; }
}