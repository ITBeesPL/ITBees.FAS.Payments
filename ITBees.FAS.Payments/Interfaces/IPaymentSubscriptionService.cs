using ITBees.FAS.Payments.Controllers;
using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSubscriptionService
{
    InitialisedPaymentLinkVm CreateNewPaymentSubscriptionSession(NewPaymentSubscriptionIm newPaymentSubscriptionIm, string paymentOperator);
    InitialisedApplePaymentVm CreateNewApplePaymentSubscriptionSession(NewApplePaymentSubscriptionIm newApplePaymentSubscriptionIm);
}