namespace ITBees.FAS.Payments.Interfaces;

public interface IFasPaymentProcessor
{
    FasActivePaymentSession CreatePaymentSession(FasPayment fasPayment, bool oneTimePayment);
    bool ConfirmPayment(Guid paymentSessionGuid);
    string ProcessorName { get; }
}