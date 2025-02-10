namespace ITBees.FAS.Payments.Controllers.Models;

public class DeactivatedSubscriptionPlanIm
{
    public Guid SubscriptionPlanGuid { get; set; }
    public bool CurrentActiveState { get; set; }
}