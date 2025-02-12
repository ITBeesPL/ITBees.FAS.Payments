using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionService
{
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm);
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewMultiPaymentIm newPaymentIm);
    bool ConfirmPayment(Guid paymentSessionGuid);
    void CancelPayment(Guid paymentSessionGuid);
    bool UserCompanyHasEverMadePayment(Guid userAccountGuid);
    bool FakeAppleConfirmPayment(Guid paymentSessionId, string operatorTransactionId);
}