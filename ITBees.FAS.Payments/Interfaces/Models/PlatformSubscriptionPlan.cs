using System.ComponentModel.DataAnnotations.Schema;
using ITBees.Models.Languages;
using ITBees.Models.Users;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class PlatformSubscriptionPlan
{
    public Guid Guid { get; set; }
    public string PlanName { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }
    public DateTime? StartFrom { get; set; }
    public UserAccount CreatedBy { get; set; }
    public Guid CreatedByGuid { get; set; }
    public int Interval { get; set; }
    public int IntervalDays { get; set; }
    public bool IsOneTimePayment { get; set; }
    public string GroupName { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal NetValue { get; set; }
    public int VatPercentage { get; set; }
    public string Title { get; set; }
    public List<PlanFeature> PlanFeatures { get; set; }
    public int Position { get; set; }
    public bool MostPopular { get; set; }
    public string? PlanDescription { get; set; }
    public string? ButtonText { get; set; }
    public string? BadgeText { get; set; }
    public Language Language { get; set; }
    public int? LanguageId { get; set; }
    public string? Currency { get; set; }
    public bool IsTrial { get; set; }
    public bool CustomImplementation { get; set; }
    public string? CustomImplementationTypeName { get; set; }
    public string? AppleProductId { get; set; }
    public string? BgColor { get; set; }
    public string? FgColor { get; set; }
}