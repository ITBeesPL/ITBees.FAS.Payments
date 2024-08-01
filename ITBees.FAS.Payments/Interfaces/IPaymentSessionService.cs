using ITBees.FAS.Payments.Controllers;
using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionService
{
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm);
    bool ConfirmPayment(Guid paymentSessionGuid);
    void CancelPayment(Guid paymentSessionGuid);
    InitialisedPaymentLinkVm CreateNewPaymentSubscriptionSession(NewPaymentSubscriptionIm newPaymentSubscriptionIm);
}