namespace ITBees.FAS.Payments;

public class FasActivePaymentSession
{
    public string SessionUrl { get; }
    public string FasPaymentSessionGuid { get; }

    public FasActivePaymentSession(string sessionUrl, string fasPaymentSessionGuid)
    {
        SessionUrl = sessionUrl;
        FasPaymentSessionGuid = fasPaymentSessionGuid;
    }
}