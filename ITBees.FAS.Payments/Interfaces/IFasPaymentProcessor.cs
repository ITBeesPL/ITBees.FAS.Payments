namespace ITBees.FAS.Payments.Interfaces;

public interface IFasPaymentProcessor
{
    FasActivePaymentSession CreatePaymentSession(FasPayment fasPayment, bool oneTimePayment, string? successUrl = "", string? failUrl = "");
    bool ConfirmPayment(Guid paymentSessionGuid);
    string ProcessorName { get; }
}