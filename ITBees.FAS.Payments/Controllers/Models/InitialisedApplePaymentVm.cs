namespace ITBees.FAS.Payments.Controllers.Models;

public class InitialisedApplePaymentVm
{
    public InitialisedApplePaymentVm()
    {
        
    }

    public InitialisedApplePaymentVm(InitialisedPaymentLinkVm paymentSession)
    {
        Guid = paymentSession.PaymentSessionGuid.Value;
    }

    public Guid Guid { get; set; }
}