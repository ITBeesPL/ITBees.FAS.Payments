namespace ITBees.FAS.Payments;

public class FasPayment
{
    public FasPaymentMode Mode { get; set; }
    public List<FasProduct> Products { get; set; }
}