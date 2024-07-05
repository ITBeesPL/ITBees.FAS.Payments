namespace ITBees.FAS.Payments.Interfaces;

public interface IFasPaymentProcessor
{
    FasActivePaymentSession CreatePaymentSession(FasPayment fasPayment);
}