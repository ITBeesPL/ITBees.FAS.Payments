namespace ITBees.FAS.Payments.Controllers;

public class InitialisedPaymentLinkVm
{
    public string SessionUrl { get; }

    public InitialisedPaymentLinkVm(string sessionUrl)
    {
        SessionUrl = sessionUrl;
    }
}