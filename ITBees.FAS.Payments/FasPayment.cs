namespace ITBees.FAS.Payments;

public class FasPayment
{
    public Guid PaymentSessionGuid { get; set; }
    public FasPaymentMode Mode { get; set; }
    public List<FasProduct> Products { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }
}