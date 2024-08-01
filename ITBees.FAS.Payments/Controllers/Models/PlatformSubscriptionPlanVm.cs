using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformSubscriptionPlanVm
{
    public PlatformSubscriptionPlanVm() { }
    public PlatformSubscriptionPlanVm(PlatformSubscriptionPlan x)
    {
        this.Title = x.Title;
        this.Guid = x.Guid;
        this.Value = x.Value;
        this.Expires = x.Expires;
        this.Interval = x.Interval;
        this.IsActive = x.IsActive;
        this.IsOneTimePayment = x.IsOneTimePayment;
        this.PlanName = x.PlanName;
        this.GroupName = x.GroupName;
        this.PlanFeatures = x.PlanFeatures.Select(x => new PlanFeatureVm(x)).ToList();
        Position = x.Position;
        MostPopular = x.MostPopular;
        PlanDescription = x.PlanDescription;
        ButtonText = x.ButtonText;
        BadgeText = x.BadgeText;
    }

    public string? BadgeText { get; set; }

    public string? ButtonText { get; set; }

    public string? PlanDescription { get; set; }

    public bool MostPopular { get; set; }

    public int Position { get; set; }

    public string Title { get; set; }

    public List<PlanFeatureVm> PlanFeatures { get; set; }

    public string GroupName { get; set; }
    public Guid Guid { get; set; }
    public decimal Value { get; set; }
    public DateTime? Expires { get; set; }
    public int Interval { get; set; }
    public bool IsActive { get; set; }
    public bool IsOneTimePayment { get; set; }
    public string PlanName { get; set; }
}