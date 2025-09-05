using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Models.Companies;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionCreator
{
    PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid, IFasPaymentProcessor paymentProcessor,
        Guid invoiceDataGuid, string paymentOperator, Guid? orderPackGuid = null);

    void CloseSuccessfulPayment(Guid guid, DateTime sessionCreated, string customerSubscriptionId = null);

    PaymentSession CreatePaymentSessionFromSubscriptionRenew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid, string paymentOperator,
        Guid? orderPackGuid = null, string? customerSubscriptionId = null,bool invoiceCreated = false);

    Company? TryGetCompanyWithSubscriptionPlanFromPaymentSubscriptionId(string stripeSubscriptionId);
}