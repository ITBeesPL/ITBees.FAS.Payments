namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentSummaryVm
{
    public List<PaymentSummaryElementVm> Payments { get; set; }
    public decimal TotalIncomGross { get; set; }
    public string CommisionPercentage { get; set; }
    public string Vat { get; set; }
    public decimal TotalIncomNetAmount { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal VatAmount { get; set; }
}