using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionService
{
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm);
    bool ConfirmPayment(Guid paymentSessionGuid);
    void CancelPayment(Guid paymentSessionGuid);
    bool UserCompanyHasEverMadePayment(Guid userAccountGuid);
}