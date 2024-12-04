using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class FinishedPaymentVm
{
    public FinishedPaymentVm()
    {
        
    }
    public FinishedPaymentVm(PaymentSession x)
    {
        Created = x.Created;
        Finished = x.Finished;
        City = x.InvoiceData.City;
        Nip = x.InvoiceData.NIP;
        Street = x.InvoiceData.Street;
        Country  =x.InvoiceData.Country;
        CompanyName = x.InvoiceData.CompanyName;
        Email = x.InvoiceData.InvoiceEmail;
        PostCode = x.InvoiceData.PostCode;
        InvoiceRequested = x.InvoiceData.InvoiceRequested;
        Amount = x.InvoiceData.SubscriptionPlan.Value;
    }

    public bool InvoiceRequested { get; set; }

    public string PostCode { get; set; }

    public decimal Amount { get; set; }

    public string Email { get; set; }

    public string CompanyName { get; set; }

    public string Country { get; set; }

    public string Street { get; set; }

    public string Nip { get; set; }

    public string City { get; set; }

    public bool Finished { get; set; }

    public DateTime Created { get; set; }
}