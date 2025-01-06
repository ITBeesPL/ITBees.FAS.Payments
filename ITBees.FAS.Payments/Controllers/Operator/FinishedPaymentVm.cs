using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Models.Languages;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class FinishedPaymentVm
{
    public FinishedPaymentVm()
    {
        
    }
    public FinishedPaymentVm(PaymentSession x)
    {
        Guid = x.Guid;
        Created = x.Created;
        Finished = x.Finished;
        City = x.InvoiceData?.City;
        Nip = x.InvoiceData?.NIP;
        Street = x.InvoiceData?.Street;
        Country  =x.InvoiceData?.Country;
        CompanyName = x.InvoiceData?.CompanyName;
        Email = x.InvoiceData?.InvoiceEmail;
        PostCode = x.InvoiceData?.PostCode;
        InvoiceRequested = x.InvoiceData?.InvoiceRequested;
        Amount = x.InvoiceData?.SubscriptionPlan?.Value.ToString("F2");
        CreatedBy = x.CreatedBy?.DisplayName;
        InvoiceProductName = x.InvoiceData?.SubscriptionPlan?.PlanName;
        InvoiceQuantity = 1;
        
    }

    public int InvoiceQuantity { get; set; }

    public string InvoiceProductName { get; set; }

    public string? CreatedBy { get; set; }

    public Guid Guid { get; set; }

    public bool? InvoiceRequested { get; set; }

    public string? PostCode { get; set; }

    public string? Amount { get; set; }

    public string? Email { get; set; }

    public string? CompanyName { get; set; }

    public string? Country { get; set; }

    public string? Street { get; set; }

    public string? Nip { get; set; }

    public string? City { get; set; }

    public bool? Finished { get; set; }

    public DateTime? Created { get; set; }
}