using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

/// <summary>
/// What the invoice of a paid order pack session says - registered by hosts selling order packs (e.g. devices with
/// their monthly service). An order pack has no platform plan, so invoice issuers (dzielnik.com, KSeF) ask the host
/// for the lines: everything bought at the checkout, only the service of the period for a subscription renewal.
/// </summary>
public interface IOrderPackInvoiceContentProvider
{
    /// <param name="paymentSession">Paid session with OrderPackGuid set (checkout or FromSubscriptionRenew).</param>
    /// <returns>null when the host does not invoice the session (e.g. it does not know the order pack).</returns>
    OrderPackInvoiceContent? GetInvoiceContent(PaymentSession paymentSession);
}

public class OrderPackInvoiceContent
{
    public List<OrderPackInvoiceLine> Lines { get; set; } = new();

    /// <summary>Currency of the prices (ISO), PLN by default.</summary>
    public string Currency { get; set; } = "PLN";

    /// <summary>Extra text for the invoice, e.g. the order number.</summary>
    public string? Notes { get; set; }
}

public class OrderPackInvoiceLine
{
    public string Name { get; set; } = string.Empty;

    /// <summary>"szt." for goods, "usł." for services.</summary>
    public string Unit { get; set; } = "szt.";

    public decimal Quantity { get; set; }
    public decimal UnitNetPrice { get; set; }
    public int VatPercentage { get; set; }
}
