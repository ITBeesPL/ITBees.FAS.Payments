using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Controllers.Models;

public class InvoiceDataVm
{
    public InvoiceDataVm()
    {
        
    }
    public InvoiceDataVm(InvoiceData x)
    {
        Guid = x.Guid;
        CompanyGuid = x.CompanyGuid;
        NIP = x.NIP;
        Street = x.Street;
        City = x.City;
        Country = x.Country;
        CompanyName = x.CompanyName;
        PostCode = x.PostCode;
        InvoiceEmail = x.InvoiceEmail;
        SubscriptionPlan = x.SubscriptionPlan?.PlanName;
        SubscriptionPlanGuid = x.SubscriptionPlan?.Guid;
    }

    public Guid? SubscriptionPlanGuid { get; set; }

    public string SubscriptionPlan { get; set; }
    public Guid Guid { get; set; }
    public Guid CompanyGuid { get; set; }
    public string NIP { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string CompanyName { get; set; }
    public string PostCode { get; set; }
    public string InvoiceEmail { get; set; }
}