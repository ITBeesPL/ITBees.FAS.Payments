namespace ITBees.FAS.Payments;

public class FasActivePaymentSession
{
    public string SessionUrl { get; }

    public FasActivePaymentSession(string sessionUrl)
    {
        SessionUrl = sessionUrl;
    }
}