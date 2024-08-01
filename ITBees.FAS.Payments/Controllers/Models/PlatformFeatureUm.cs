namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformFeatureUm
{
    public int Id { get; set; }
    public string? FeatureDescription { get; set; }
    public string FeatureName { get; set; }
    public bool IsVisible { get; set; }
}