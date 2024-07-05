namespace ITBees.FAS.Payments;

public class FasProduct
{
    public Guid Guid { get; set; }
    public string PaymentTitleOrProductName { get; set; }
    public long Quantity { get; set; }
    public string Currency { get; set; }
    public FasBillingPeriod BillingPeriod { get; set; }
}