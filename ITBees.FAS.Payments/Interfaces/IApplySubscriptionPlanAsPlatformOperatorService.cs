using ITBees.FAS.Payments.Controllers;
using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IApplySubscriptionPlanAsPlatformOperatorService
{
    ApplySubscriptionPlanResultVm Apply(ApplySubscriptionPlanToCompanyIm applySubscriptionPlanToCompanyIm);

    /// <param name="allowUsedTrial">
    /// Applies a trial plan to a company that has already used its trial (e.g. when a complaint is accepted).
    /// Only the platform operator may do it - POST /ApplySubscriptionPlan passes true; a user enabling a trial
    /// on their own goes through the overload above and is refused.
    /// </param>
    ApplySubscriptionPlanResultVm Apply(ApplySubscriptionPlanToCompanyIm applySubscriptionPlanToCompanyIm, bool allowUsedTrial);
    void Delete(Guid companyGuid);
}