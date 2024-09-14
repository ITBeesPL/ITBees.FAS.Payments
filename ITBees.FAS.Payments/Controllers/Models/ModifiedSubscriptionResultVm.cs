namespace ITBees.FAS.Payments.Controllers.Models;

public class ModifiedSubscriptionResultVm
{
    public DateTime PlanActiveTo { get; set; }
    public string? PlanName { get; set; }
    public Guid? PlanGuid { get; set; }
}