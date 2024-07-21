using ITBees.Models.Companies;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class SelectedSubscriptionPlan
{
    public Guid Guid { get; set; }
    public Company Company { get; set; }
    public Guid CompanyGuid { get; set; }
    public PlatformSubscriptionPlan PlatformSubscriptionPlan { get; set; }
    public Guid PlatformSubscriptionPlanGuid { get; set; }
    public bool IsActive { get; set; }
    public DateTime? Expired { get; set; }
}