namespace ITBees.FAS.Payments.Controllers.Models;

public class InitialisedPaymentLinkVm
{
    public InitialisedPaymentLinkVm() { }
    public string SessionUrl { get; }

    public InitialisedPaymentLinkVm(string sessionUrl)
    {
        SessionUrl = sessionUrl;
    }
}