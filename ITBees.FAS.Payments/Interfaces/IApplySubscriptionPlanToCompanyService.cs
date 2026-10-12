using ITBees.Models.Payments;

namespace ITBees.FAS.Payments.Interfaces;

public interface IApplySubscriptionPlanToCompanyService
{
    void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid, DateTime startingFrom);

    /// <param name="allowUsedTrial">
    /// True skips the check that the company has not used its trial yet (TrialPlanAlreadyUsed) - for the
    /// platform operator only, see IApplySubscriptionPlanAsPlatformOperatorService.
    /// </param>
    void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid, DateTime startingFrom, bool allowUsedTrial);
    void Revoke(Guid companyGuid);
}