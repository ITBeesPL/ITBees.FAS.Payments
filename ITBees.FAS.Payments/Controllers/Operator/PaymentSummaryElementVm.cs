namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentSummaryElementVm
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string PaymentOperator { get; set; }
    public int TotalSuccessCount { get; set; }
    public int TotalInvoicesCreated { get; set; }
    public decimal TotalGrossAmount { get; set; }
    public decimal SubscriptionValue { get; set; }
}