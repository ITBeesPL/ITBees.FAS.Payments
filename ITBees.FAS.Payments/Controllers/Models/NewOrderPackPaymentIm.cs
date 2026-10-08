namespace ITBees.FAS.Payments.Controllers.Models;

/// <summary>
/// Payment of an order pack with several products. When any product is recurring (FasProduct.IsRecurring),
/// the payment becomes a subscription: recurring products are charged every period, one-time products only
/// with the first one. All recurring products must share the same billing period.
/// </summary>
public class NewOrderPackPaymentIm
{
    public Guid OrderPackGuid { get; set; }
    public Guid InvoiceDataGuid { get; set; }

    /// <summary>Stored with the payment operator, so the subscription can be managed by the company.</summary>
    public Guid? CompanyGuid { get; set; }

    /// <summary>Pre-filled on the payment page and used by the operator for receipts.</summary>
    public string? CustomerEmail { get; set; }

    public List<FasProduct> Products { get; set; } = new();
    public string? SuccessUrl { get; set; }
    public string? FailureUrl { get; set; }
}
