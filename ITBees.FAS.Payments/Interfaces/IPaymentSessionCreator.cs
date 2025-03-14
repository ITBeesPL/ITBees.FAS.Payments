using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionCreator
{
    PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid, IFasPaymentProcessor paymentProcessor,
        Guid invoiceDataGuid, string paymentOperator, Guid? orderPackGuid = null);
    void CloseSuccessfulPayment(Guid guid);
}