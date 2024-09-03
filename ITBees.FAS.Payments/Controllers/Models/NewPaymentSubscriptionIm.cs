namespace ITBees.FAS.Payments.Controllers.Models;

public class NewPaymentSubscriptionIm
{
    public Guid CompanyGuid { get; set; }
    public Guid PlatformSubscriptionPlanGuid { get; set; }
    public Guid InvoiceDataGuid { get; set; }
    public string? SuccessUrl { get; set; }
    public string? FailureUrl { get; set; }
}