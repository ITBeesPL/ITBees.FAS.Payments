using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Subscriptions;

class SelectedSubscriptionPlansService : ISubscriptionPlansService
{
    private readonly IWriteOnlyRepository<SelectedSubscriptionPlan> _selectedSubPlanRwRepo;
    private readonly IReadOnlyRepository<SelectedSubscriptionPlan> _selectedSubPlanRoRepo;
    private readonly IAspCurrentUserService _aspCurrentUserService;

    public SelectedSubscriptionPlansService(
        IWriteOnlyRepository<SelectedSubscriptionPlan> selectedSubPlanRwRepo,
        IReadOnlyRepository<SelectedSubscriptionPlan> selectedSubPlanRoRepo,
        IAspCurrentUserService aspCurrentUserService)
    {
        _selectedSubPlanRwRepo = selectedSubPlanRwRepo;
        _selectedSubPlanRoRepo = selectedSubPlanRoRepo;
        _aspCurrentUserService = aspCurrentUserService;
    }
    public SelectedSubscriptionPlanVm CreateNew(SelectedSubscriptionPlanIm selectedSubscriptionPlanIm)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _selectedSubPlanRwRepo.InsertData(new SelectedSubscriptionPlan()
        {
            CompanyGuid = selectedSubscriptionPlanIm.CompanyGuid,
            PlatformSubscriptionPlanGuid = selectedSubscriptionPlanIm.PlatformSubscriptionPlanGuid,
        });

        var resultData = _selectedSubPlanRoRepo.GetData(x => x.Guid == result.Guid, x => x.PlatformSubscriptionPlan).First();

        return new SelectedSubscriptionPlanVm() { Guid = result.Guid, PlanName = resultData.PlatformSubscriptionPlan.PlanName };
    }

    public SelectedSubscriptionPlanVm Update(SelectedSubscriptionPlanUm selectedSubscriptionPlanUm)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _selectedSubPlanRwRepo.UpdateData(x => x.Guid == selectedSubscriptionPlanUm.Guid, x =>
        {
            x.PlatformSubscriptionPlanGuid = selectedSubscriptionPlanUm.Guid;
        }, x => x.PlatformSubscriptionPlan).First();

        return new SelectedSubscriptionPlanVm() { Guid = result.Guid, PlanName = result.PlatformSubscriptionPlan.PlanName };

    }

    public void Delete(Guid selectedSubscriptionPlanGuid)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _selectedSubPlanRwRepo.DeleteData(x => x.Guid == selectedSubscriptionPlanGuid);
    }

    public SelectedSubscriptionPlanVm Get(Guid companyGuid)
    {
        //todo security - check user can create plan for this company
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        var result = _selectedSubPlanRoRepo.GetData(x => x.CompanyGuid == companyGuid && x.IsActive,x =>
            x.PlatformSubscriptionPlan).First();

        return new SelectedSubscriptionPlanVm() { Guid = result.Guid, PlanName = result.PlatformSubscriptionPlan.PlanName };
    }
}
