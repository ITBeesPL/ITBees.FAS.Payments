namespace ITBees.FAS.Payments.Interfaces;

/// <summary>
/// Optional hook for subscriptions bought for an order pack (FasPayment.OrderPackGuid), e.g. a monthly service
/// of a purchased device. Such a subscription is not a platform plan: its renewals and refunds never touch the
/// company's plan. Register an implementation in the host to track them. Implementations must be idempotent -
/// payment operators deliver events at least once.
/// </summary>
public interface IOrderPackSubscriptionEventsHandler
{
    /// <summary>A renewal period of the order pack subscription was paid.</summary>
    void SubscriptionRenewed(OrderPackSubscriptionRenewal renewal);

    /// <summary>A payment of the order pack (first payment or a renewal) was refunded.</summary>
    void PaymentRefunded(Guid orderPackGuid, decimal amount, string currency, bool isFullRefund)
    {
    }

    /// <summary>
    /// A payment of the order pack was refunded - with the payment operator's references, so a refund the host
    /// started itself can be told apart from one made outside it (e.g. in the Stripe dashboard).
    /// </summary>
    void PaymentRefunded(OrderPackRefund refund)
    {
        PaymentRefunded(refund.OrderPackGuid, refund.Amount, refund.Currency, refund.IsFullRefund);
    }

    /// <summary>The order pack subscription ended (cancelled by the customer, the operator or for non-payment).</summary>
    void SubscriptionEnded(Guid orderPackGuid, string subscriptionId, DateTime endedAt)
    {
    }
}

public class OrderPackSubscriptionRenewal
{
    public Guid OrderPackGuid { get; set; }

    /// <summary>Payment session created for the renewal (FromSubscriptionRenew).</summary>
    public Guid? RenewalPaymentSessionGuid { get; set; }

    public string SubscriptionId { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }
    public decimal AmountPaid { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
}

public class OrderPackRefund
{
    public Guid OrderPackGuid { get; set; }

    /// <summary>Refunded amount of this event (the whole refunded amount of the charge for charge.refunded).</summary>
    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    /// <summary>The whole charge is refunded now.</summary>
    public bool IsFullRefund { get; set; }

    public string? RefundId { get; set; }
    public string? ChargeId { get; set; }
    public string? PaymentIntentId { get; set; }
}
