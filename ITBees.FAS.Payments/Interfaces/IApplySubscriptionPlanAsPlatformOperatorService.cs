using ITBees.FAS.Payments.Controllers;
using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IApplySubscriptionPlanAsPlatformOperatorService
{
    ApplySubscriptionPlanResultVm Apply(ApplySubscriptionPlanToCompanyIm applySubscriptionPlanToCompanyIm);
    void Delete(Guid companyGuid);
}