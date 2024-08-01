using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.UserManager.Interfaces.Services;

namespace ITBees.FAS.Payments.Subscriptions;

class PlatformAvailableSubscriptionPlansService : IPlatformAvailableSubscriptionPlansService
{
    private readonly IWriteOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRwRepo;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoPlan;
    private readonly IAspCurrentUserService _aspCurrentUserService;

    public PlatformAvailableSubscriptionPlansService(IWriteOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRwRepo,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoPlan,
        IAspCurrentUserService aspCurrentUserService)
    {
        _platformSubscriptionPlanRwRepo = platformSubscriptionPlanRwRepo;
        _platformSubscriptionPlanRoPlan = platformSubscriptionPlanRoPlan;
        _aspCurrentUserService = aspCurrentUserService;
    }
    public PlatformSubscriptionPlanVm CreateNew(PlatformSubscriptionPlanIm selectedSubscriptionPlanIm)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _platformSubscriptionPlanRwRepo.InsertData(new PlatformSubscriptionPlan()
        {
            Value = selectedSubscriptionPlanIm.Value,
            Created = DateTime.Now,
            CreatedByGuid = currentUser.Guid,
            Expires = selectedSubscriptionPlanIm.Expires,
            Interval = selectedSubscriptionPlanIm.Interval,
            IsActive = selectedSubscriptionPlanIm.IsActive,
            IsOneTimePayment = selectedSubscriptionPlanIm.IsOneTimePayment,
            PlanName = selectedSubscriptionPlanIm.PlanName,
            GroupName = selectedSubscriptionPlanIm.GroupName
        });

        var resultData = _platformSubscriptionPlanRoPlan.GetData(x => x.Guid == result.Guid, x => x.CreatedBy).First();

        return new PlatformSubscriptionPlanVm(resultData);
    }

    public List<PlatformSubscriptionPlanVm> GetAllActivePlans()
    {
        var result = _platformSubscriptionPlanRoPlan.GetData(x => x.IsActive, x=>x.PlanFeatures).ToList();

        return result.Select(x=>new PlatformSubscriptionPlanVm(x)).ToList();
    }

    public PlatformSubscriptionPlanVm Update(PlatformSubscriptionPlanUm selectedSubscriptionPlanIm)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _platformSubscriptionPlanRwRepo.UpdateData(x => x.Guid == selectedSubscriptionPlanIm.Guid, x =>
        {
            x.Value = selectedSubscriptionPlanIm.Value;
            x.Expires = selectedSubscriptionPlanIm.Expires;
            x.Interval = selectedSubscriptionPlanIm.Interval;
            x.IsActive = selectedSubscriptionPlanIm.IsActive;
            x.IsOneTimePayment = selectedSubscriptionPlanIm.IsOneTimePayment;
            x.PlanName = selectedSubscriptionPlanIm.PlanName;
            x.GroupName = selectedSubscriptionPlanIm.GroupName;
        }, plan => plan.CreatedBy).First();

        return new PlatformSubscriptionPlanVm(result);

    }

    public void Delete(Guid platformSubscriptionPlanGuid)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _platformSubscriptionPlanRwRepo.DeleteData(x => x.Guid == platformSubscriptionPlanGuid);
    }

    public PlatformSubscriptionPlanVm Get(Guid selectedSubscriptionPlanGuid)
    {
        //todo security - check user can create plan for this company
        var result = _platformSubscriptionPlanRoPlan.GetData(x => x.Guid == selectedSubscriptionPlanGuid).First();

        return new PlatformSubscriptionPlanVm(result);
    }
}