using ITBees.FAS.Payments.Interfaces;

namespace ITBees.FAS.Payments
{
    public class FasPaymentManager : IFasPaymentManager
    {
        private readonly IFasPaymentProcessor _defaultPaymentProcessor;

        public FasPaymentManager(IFasPaymentProcessor defaultPaymentProcessor)
        {
            _defaultPaymentProcessor = defaultPaymentProcessor;
        }

        public FasActivePaymentSession CreatePayment(FasPayment fasPayment, IFasPaymentProcessor paymentProcessor)
        {
            return paymentProcessor.CreatePaymentSession(fasPayment);
        }

        public FasActivePaymentSession CreatePayment(FasPayment fasPayment)
        {
            return _defaultPaymentProcessor.CreatePaymentSession(fasPayment);
        }
    }
}