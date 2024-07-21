using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Subscriptions;

namespace ITBees.FAS.Payments.Interfaces;

public interface ISubscriptionPlansService
{
    SelectedSubscriptionPlanVm CreateNew(SelectedSubscriptionPlanIm selectedSubscriptionPlanIm);
    SelectedSubscriptionPlanVm Update(SelectedSubscriptionPlanUm selectedSubscriptionPlanUm);
    void Delete(Guid selectedSubscriptionPlanGuid);
    SelectedSubscriptionPlanVm Get(Guid selectedSubscriptionPlanGuid);
}