using ITBees.Models.Companies;
using ITBees.Models.Users;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class InvoiceData
{
    public Guid Guid { get; set; }
    public Company Company { get; set; }
    public Guid CompanyGuid { get; set; }
    public string NIP { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string CompanyName { get; set; }
    public string PostCode { get; set; }
    public string InvoiceEmail { get; set; }
    public UserAccount CreatedBy { get; set; }
    public Guid CreatedByGuid { get; set; }
    public DateTime Created { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public UserAccount ModifiedBy { get; set; }
    public Guid? ModifiedByGuid { get; set; }
    public PlatformSubscriptionPlan SubscriptionPlan { get; set; }
    public Guid? SubscriptionPlanGuid { get; set; }
    public bool IsActive { get; set; }
    public bool InvoiceRequested { get; set; }
}