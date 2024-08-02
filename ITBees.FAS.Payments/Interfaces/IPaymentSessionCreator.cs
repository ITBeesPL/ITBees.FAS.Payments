using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionCreator
{
    PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid, IFasPaymentProcessor paymentProcessor);
    void CloseSuccessfulPayment(Guid guid);
}