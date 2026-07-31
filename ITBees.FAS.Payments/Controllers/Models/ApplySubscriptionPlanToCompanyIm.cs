namespace ITBees.FAS.Payments.Controllers.Models;

public class ApplySubscriptionPlanToCompanyIm
{
    public Guid SubscriptionPlanGuid { get; set; }
    public Guid CompanyGuid { get; set; }
    public DateTime StartingFrom { get; set; }
    /// <summary>
    /// When set, overrides the subscription end date calculated from the plan interval.
    /// </summary>
    public DateTime? ValidTo { get; set; }
}