﻿namespace ITBees.FAS.Payments.Controllers.Models;

public class PlatformSubscriptionPlanUm
{
    public Guid Guid { get; set; }
    public decimal Value { get; set; }
    public int VatPercentage { get; set; }
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
    public bool IsTrial { get; set; }
    public bool CustomImplementation { get; set; }
    public string? CustomImplementationTypeName { get; set; }
    public string BgColor { get; set; }
    public string FgColor { get; set; }
    public string? AppleProductId { get; set; }
}