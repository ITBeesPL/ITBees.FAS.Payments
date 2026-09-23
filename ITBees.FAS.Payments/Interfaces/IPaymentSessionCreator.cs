using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Models.Companies;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionCreator
{
    PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid, IFasPaymentProcessor paymentProcessor,
        Guid invoiceDataGuid, string paymentOperator, Guid? orderPackGuid = null);

    void CloseSuccessfulPayment(Guid guid, DateTime sessionCreated, string customerSubscriptionId,
        string paymentEventId = null);

    /// <param name="paidSubscriptionPlanGuid">
    /// Plan recorded by the payment operator at checkout (e.g. Stripe metadata). When set, it is applied
    /// instead of the plan currently on the session's (shared) invoice data.
    /// </param>
    void CloseSuccessfulPayment(Guid guid, DateTime sessionCreated, string customerSubscriptionId,
        string paymentEventId, Guid? paidSubscriptionPlanGuid);

    PaymentSession CreatePaymentSessionFromSubscriptionRenew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid, string paymentOperator, string paymentEventId,
        Guid? orderPackGuid = null, string? customerSubscriptionId = null,bool invoiceCreated = false);

    Company? TryGetCompanyWithSubscriptionPlanFromPaymentSubscriptionId(string stripeSubscriptionId);
}