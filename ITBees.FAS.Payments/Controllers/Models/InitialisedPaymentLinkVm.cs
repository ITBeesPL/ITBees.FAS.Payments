namespace ITBees.FAS.Payments.Controllers.Models;

public class InitialisedPaymentLinkVm
{
    public InitialisedPaymentLinkVm() { }
    public string SessionUrl { get; }

    public InitialisedPaymentLinkVm(string sessionUrl, Guid? paymentSessionGuid)
    {
        SessionUrl = sessionUrl;
        PaymentSessionGuid = paymentSessionGuid;
    }

    /// <summary>
    /// This property could be null for trial plans
    /// </summary>
    public Guid? PaymentSessionGuid { get; set; }
}