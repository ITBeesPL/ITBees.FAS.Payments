using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Controllers.Models;

public class PaymentVm
{
    public PaymentVm()
    {

    }

    public PaymentVm(PaymentSession x)
    {
        PaymentSessionGuid = x.Guid;
        Created = x.Created;
        Finished = x.Finished;
        Success = x.Success;
        FinishedDate = x.FinishedDate;
        OperatorTransactionId = x.OperatorTransactionId;
        Email = x.CreatedBy.Email;
        Value = x.InvoiceData.SubscriptionPlan == null ? 0 : x.InvoiceData.SubscriptionPlan.Value;
    }

    public decimal Value { get; set; }

    public Guid PaymentSessionGuid { get; set; }
    public DateTime Created { get; set; }
    public bool Finished { get; set; }
    public bool Success { get; set; }
    public string PaymentOperator { get; set; }
    public DateTime? FinishedDate { get; set; }
    public string? OperatorTransactionId { get; set; }
    public string Email { get; set; }
}