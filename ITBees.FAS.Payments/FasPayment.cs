namespace ITBees.FAS.Payments;

public class FasPayment
{
    public Guid PaymentSessionGuid { get; set; }
    public FasPaymentMode Mode { get; set; }
    public List<FasProduct> Products { get; set; }
}