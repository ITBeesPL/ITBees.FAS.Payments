namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformSubscriptionPlanUm
{
    public Guid Guid { get; set; }
    public decimal Value { get; set; }
    public DateTime? Expires { get; set; }
    public int Interval { get; set; }
    public bool IsActive { get; set; }
    public bool IsOneTimePayment { get; set; }
    public string PlanName { get; set; }
    public string GroupName { get; set; }
    public string Title { get; set; }
}