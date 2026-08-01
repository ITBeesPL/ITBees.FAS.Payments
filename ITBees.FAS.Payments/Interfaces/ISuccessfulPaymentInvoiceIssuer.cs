using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

/// <summary>
/// Optional hook invoked after a payment session is successfully closed (payment confirmed by the operator).
/// Register an implementation in the host application to issue an invoice (e.g. KSeF e-invoice) for the paid
/// session. When no implementation is registered, payment closing works as before.
/// Implementations must be fast and non-blocking (queue the actual work) — this is called from the payment
/// operator webhook pipeline. Use PaymentSession.InvoiceCreated as the idempotency guard, because a payment
/// can be closed both by the webhook and by the browser-redirect confirmation path.
/// </summary>
public interface ISuccessfulPaymentInvoiceIssuer
{
    /// <param name="paymentSession">Closed session with InvoiceData and InvoiceData.SubscriptionPlan loaded.</param>
    void IssueInvoiceForPaidSession(PaymentSession paymentSession);
}
