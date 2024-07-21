namespace ITBees.FAS.Payments.Controllers.Models;

public class InvoiceDataIm
{
    public Guid CompanyGuid { get; set; }
    public string NIP { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string CompanyName { get; set; }
    public string PostCode { get; set; }
    public string InvoiceEmail { get; set; }
    public Guid SubscriptionPlanGuid { get; set; }
}