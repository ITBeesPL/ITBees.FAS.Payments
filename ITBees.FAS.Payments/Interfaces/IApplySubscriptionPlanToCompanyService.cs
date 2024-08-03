using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IApplySubscriptionPlanToCompanyService
{
    void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid);
}