namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformSubscriptionPlanIm
{
    public decimal Value { get; set; }
    public DateTime? Expires { get; set; }
    public int Interval { get; set; }
    public int IntervalDays { get; set; }
    public bool IsActive { get; set; }
    public bool IsOneTimePayment { get; set; }
    public string PlanName { get; set; }
    public string GroupName { get; set; }
    public string Title { get; set; }
    public int Position { get; set; }
    public bool MostPopular { get; set; }
    public string? PlanDescription { get; set; }
    public string? ButtonText { get; set; }
    public string? BadgeText { get; set; }
    public int LanguageId { get; set; }
    public string Currency { get; set; }
}