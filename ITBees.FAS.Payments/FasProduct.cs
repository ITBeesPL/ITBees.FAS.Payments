namespace ITBees.FAS.Payments;

public class FasProduct
{
    public Guid Guid { get; set; }
    public string PaymentTitleOrProductName { get; set; }
    public long Quantity { get; set; }
    public string Currency { get; set; }
    public FasBillingPeriod BillingPeriod { get; set; }
    public decimal Price { get; set; }
    /// <summary>
    /// use month, year, etc
    /// </summary>
    public string Interval { get; set; }
    /// <summary>
    /// maximum number of repetitions is 3 years
    /// </summary>
    public int IntervalCount { get; set; }

    /// <summary>
    /// Charged on every billing period (subscription item) or only once. Null keeps the old behaviour - all
    /// products follow the payment (oneTimePayment flag of the processor). A subscription payment may mix
    /// recurring products with one-time ones (e.g. a device plus its monthly service): the one-time products
    /// are charged only with the first period.
    /// </summary>
    public bool? IsRecurring { get; set; }
}