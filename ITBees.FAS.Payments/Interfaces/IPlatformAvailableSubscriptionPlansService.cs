using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPlatformAvailableSubscriptionPlansService
{
    PlatformSubscriptionPlanVm Get(Guid selectedSubscriptionPlanGuid);
    void Delete(Guid platformSubscriptionPlanGuid);
    PlatformSubscriptionPlanVm Update(PlatformSubscriptionPlanUm selectedSubscriptionPlanIm);
    PlatformSubscriptionPlanVm CreateNew(PlatformSubscriptionPlanIm selectedSubscriptionPlanIm);
    IAsyncEnumerable<PlatformSubscriptionPlanVm> GetAllActivePlans(string acceptLanguage);
}