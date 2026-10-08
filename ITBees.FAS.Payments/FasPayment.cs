namespace ITBees.FAS.Payments;

public class FasPayment
{
    public Guid PaymentSessionGuid { get; set; }
    public FasPaymentMode Mode { get; set; }
    public List<FasProduct> Products { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }

    /// <summary>
    /// Platform subscription plan bought with this payment. Payment processors store it with the
    /// operator (e.g. Stripe subscription metadata), so renewals can be mapped back to the plan
    /// that is really being paid for instead of the company's current plan.
    /// </summary>
    public Guid? SubscriptionPlanGuid { get; set; }

    /// <summary>
    /// Company the payment is made for (stored with the operator next to the plan).
    /// </summary>
    public Guid? CompanyGuid { get; set; }

    /// <summary>
    /// Order pack paid with this payment (stored with the operator, e.g. Stripe metadata). A subscription bought
    /// for an order pack (e.g. a device service) is not a platform plan - its renewals and refunds are passed to
    /// IOrderPackSubscriptionEventsHandler instead of extending or revoking the company's plan.
    /// </summary>
    public Guid? OrderPackGuid { get; set; }
}