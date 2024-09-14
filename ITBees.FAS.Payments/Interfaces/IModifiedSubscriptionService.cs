using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IModifiedSubscriptionService
{
    ModifiedSubscriptionResultVm Modify(Guid companyGuid, DateTime validTo, string authKey);
}