using ITBees.Models.Companies;
using ITBees.Models.Users;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class PaymentSession
{
    public Guid Guid { get; set; }
    public UserAccount CreatedBy { get; set; }
    public Guid? CreatedByGuid { get; set; }
    public DateTime Created { get; set; }
    public bool Finished { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string PaymentOperator { get; set; }
    public InvoiceData InvoiceData { get; set; }
    public Guid? InvoiceDataGuid { get; set; }
    public DateTime? FinishedDate { get; set; }
    public string? OperatorTransactionId { get; set; }
    public bool InvoiceCreated { get; set; }
    public OrderPack OrderPack { get; set; }
    public Guid? OrderPackGuid { get; set; }
    public bool FromSubscriptionRenew { get; set; }
}