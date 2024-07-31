namespace ITBees.FAS.Payments.Interfaces.Models;

public class PlatformFeature
{
    public int Id { get; set; }
    public string FeatureName { get; set; }
    public string FeatureDescription { get; set; }
    public bool IsVisible { get; set; }
}