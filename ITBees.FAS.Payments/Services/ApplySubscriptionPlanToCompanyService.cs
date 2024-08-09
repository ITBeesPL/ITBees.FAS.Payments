using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

class ApplySubscriptionPlanToCompanyService : IApplySubscriptionPlanToCompanyService
{
    private readonly IWriteOnlyRepository<Company> _companyRwRepo;
    private readonly ILogger<ApplySubscriptionPlanToCompanyService> _logger;

    public ApplySubscriptionPlanToCompanyService(IWriteOnlyRepository<Company> companyRwRepo, ILogger<ApplySubscriptionPlanToCompanyService> logger)
    {
        _companyRwRepo = companyRwRepo;
        _logger = logger;
    }

    public void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid)
    {
        _companyRwRepo.UpdateData(x => x.Guid == companyGuid, x =>
        {
            if (subscriptionPlan.IsTrial)
            {
                x.CompanyPlatformSubscription.TrialNotAvailable = true;
            }

            x.CompanyPlatformSubscription.SubscriptionPlanGuid = subscriptionPlan.Guid;
            x.CompanyPlatformSubscription.SubscriptionPlanName = subscriptionPlan.PlanName;
            x.CompanyPlatformSubscription.SubscriptionActiveTo = DateTime.Now.AddMonths(subscriptionPlan.Interval).AddDays(subscriptionPlan.IntervalDays);
        });
    }
}