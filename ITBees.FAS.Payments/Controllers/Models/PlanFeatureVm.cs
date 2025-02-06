using ITBees.Models.Payments;

namespace ITBees.FAS.Payments.Controllers.Models;

public class PlanFeatureVm
{
    public PlanFeatureVm() { }
    public PlanFeatureVm(PlanFeature x)
    {
        IsActive = x.IsActive;
        Position = x.Position;
        FeatureName = x.PlatformFeature.FeatureName;
        FeatureName = x.PlatformFeature.FeatureDescription;
        PlatformFeatureId = x.PlatformFeature.Id;
        PlanFeatureId = x.Id;
        IsAvailable = x.IsAvailable;
        Description = x.Description;
    }

    public string? Description { get; set; }

    public bool IsAvailable { get; set; }

    public int PlanFeatureId { get; set; }

    public int PlatformFeatureId { get; set; }

    public string FeatureName { get; set; }


    public int Position { get; set; }

    public bool IsActive { get; set; }
}