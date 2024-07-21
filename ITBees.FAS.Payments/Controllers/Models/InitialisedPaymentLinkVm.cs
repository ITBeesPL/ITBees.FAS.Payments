namespace ITBees.FAS.Payments.Controllers.Models;

public class InitialisedPaymentLinkVm
{
    public string SessionUrl { get; }

    public InitialisedPaymentLinkVm(string sessionUrl)
    {
        SessionUrl = sessionUrl;
    }
}