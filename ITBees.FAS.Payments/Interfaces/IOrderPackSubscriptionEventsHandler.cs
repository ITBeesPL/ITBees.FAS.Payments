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
