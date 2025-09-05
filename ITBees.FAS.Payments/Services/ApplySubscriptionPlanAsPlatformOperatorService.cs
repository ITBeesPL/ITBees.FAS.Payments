using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Payments;

namespace ITBees.FAS.Payments.Services;

class ApplySubscriptionPlanAsPlatformOperatorService : IApplySubscriptionPlanAsPlatformOperatorService
{
    private readonly IApplySubscriptionPlanToCompanyService _applySubscriptionPlanToCompanyService;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoRepo;
    private readonly IWriteOnlyRepository<Company> _companyRwRepo;

    public ApplySubscriptionPlanAsPlatformOperatorService(
        IApplySubscriptionPlanToCompanyService applySubscriptionPlanToCompanyService,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoRepo,
        IWriteOnlyRepository<Company> companyRwRepo
        )
    {
        _applySubscriptionPlanToCompanyService = applySubscriptionPlanToCompanyService;
        _platformSubscriptionPlanRoRepo = platformSubscriptionPlanRoRepo;
        _companyRwRepo = companyRwRepo;
    }
    public ApplySubscriptionPlanResultVm Apply(ApplySubscriptionPlanToCompanyIm applySubscriptionPlanToCompanyIm)
    {
        var subscriptionPlan = _platformSubscriptionPlanRoRepo.GetFirst(x=>x.Guid == applySubscriptionPlanToCompanyIm.SubscriptionPlanGuid);
        try
        {
            _applySubscriptionPlanToCompanyService.Apply(subscriptionPlan, applySubscriptionPlanToCompanyIm.CompanyGuid, applySubscriptionPlanToCompanyIm.StartingFrom);
            return new ApplySubscriptionPlanResultVm() { Success = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApplySubscriptionPlanResultVm() { Success = false, Message = e.Message};
        }
    }

    public void Delete(Guid companyGuid)
    {
        _companyRwRepo.UpdateData(x => x.Guid == companyGuid, x =>
        {
            x.CompanyPlatformSubscription.SubscriptionPlanGuid = null;
            x.CompanyPlatformSubscription.SubscriptionActiveTo = null;
            x.CompanyPlatformSubscription.SubscriptionPlanName = null;
        });
    }
}