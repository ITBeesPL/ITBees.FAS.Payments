using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformFeatureVm
{
    public PlatformFeatureVm(PlatformFeature x)
    {
        FeatureDescription = x.FeatureDescription;
        FeatureName = x.FeatureName;
        Id = x.Id;
        IsVisible = x.IsVisible;
        FeatureValueDescription = x.FeatureValueDescription;
        GroupName = x.GroupName;
    }

    public string? GroupName { get; set; }

    public string? FeatureValueDescription { get; set; }

    public bool IsVisible { get; set; }

    public int Id { get; set; }

    public string FeatureName { get; set; }

    public string FeatureDescription { get; set; }
}