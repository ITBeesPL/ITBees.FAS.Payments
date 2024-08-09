using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Languages;
using ITBees.Translations;
using ITBees.UserManager.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

class ApplySubscriptionPlanToCompanyService : IApplySubscriptionPlanToCompanyService
{
    private readonly IWriteOnlyRepository<Company> _companyRwRepo;
    private readonly ILogger<ApplySubscriptionPlanToCompanyService> _logger;
    private readonly IReadOnlyRepository<Company> _companyRoRepo;
    private readonly IAspCurrentUserService _aspCurrentUserService;

    public ApplySubscriptionPlanToCompanyService(
        IWriteOnlyRepository<Company> companyRwRepo,
        ILogger<ApplySubscriptionPlanToCompanyService> logger,
        IReadOnlyRepository<Company> companyRoRepo,
        IAspCurrentUserService aspCurrentUserService)
    {
        _companyRwRepo = companyRwRepo;
        _logger = logger;
        _companyRoRepo = companyRoRepo;
        _aspCurrentUserService = aspCurrentUserService;
    }

    public void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid)
    {

        if (subscriptionPlan.IsTrial)
        {
            var company = _companyRoRepo.GetFirst(x => x.Guid == companyGuid);
            if (company.CompanyPlatformSubscription.TrialNotAvailable)
            {
                var language = _aspCurrentUserService.GetCurrentUser() == null ? new En() : _aspCurrentUserService.GetCurrentUser().Language;
                throw new ArgumentException(Translate.Get(() => Translations.ApplySubscriptionPlan.Errors.TrialPlanAlreadyUsed, language));
            }
        }
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