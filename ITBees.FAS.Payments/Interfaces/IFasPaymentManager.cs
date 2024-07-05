namespace ITBees.FAS.Payments.Interfaces;

public interface IFasPaymentManager
{
    FasActivePaymentSession CreatePayment(FasPayment fasPayment, IFasPaymentProcessor paymentProcessor);
}