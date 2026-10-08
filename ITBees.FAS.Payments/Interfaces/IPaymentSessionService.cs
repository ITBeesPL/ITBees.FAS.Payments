using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionService
{
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm);
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewMultiPaymentIm newPaymentIm);

    /// <summary>
    /// Payment of an order pack with several products, optionally mixing one-time and recurring ones
    /// (then it is a subscription). Closing it calls IOrderPackFinalizerService.
    /// </summary>
    InitialisedPaymentLinkVm CreateOrderPackPaymentSession(NewOrderPackPaymentIm newOrderPackPaymentIm);
    bool ConfirmPayment(Guid paymentSessionGuid);
    void CancelPayment(Guid paymentSessionGuid);
    bool UserCompanyHasEverMadePayment(Guid userAccountGuid);
    bool FakeAppleConfirmPayment(Guid paymentSessionId, string operatorTransactionId);
}