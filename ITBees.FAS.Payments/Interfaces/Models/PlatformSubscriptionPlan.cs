using ITBees.Models.Users;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class PlatformSubscriptionPlan
{
    public Guid Guid { get; set; }
    public string PlanName { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }
    public UserAccount CreatedBy { get; set; }
    public Guid CreatedByGuid { get; set; }
    public int Interval { get; set; }
    public bool IsOneTimePayment { get; set; }
    public string GroupName { get; set; }
    public decimal Value { get; set; }
}