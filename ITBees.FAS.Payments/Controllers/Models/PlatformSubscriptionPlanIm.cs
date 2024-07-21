namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformSubscriptionPlanIm
{
    public decimal Value { get; set; }
    public DateTime? Expires { get; set; }
    public int Interval { get; set; }
    public bool IsActive { get; set; }
    public bool IsOneTimePayment { get; set; }
    public string PlanName { get; set; }
    public string GroupName { get; set; }
}