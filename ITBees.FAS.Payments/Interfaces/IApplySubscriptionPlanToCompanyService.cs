using ITBees.Models.Payments;

namespace ITBees.FAS.Payments.Interfaces;

public interface IApplySubscriptionPlanToCompanyService
{
    void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid);
}