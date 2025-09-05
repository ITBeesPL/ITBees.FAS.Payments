using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Languages;
using ITBees.Models.Payments;
using ITBees.Translations;
using ITBees.UserManager.Interfaces;
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

    public void Apply(PlatformSubscriptionPlan subscriptionPlan, Guid companyGuid, DateTime startingFrom)
    {

        if (subscriptionPlan.IsTrial)
        {
            var company = _companyRoRepo.GetFirst(x => x.Guid == companyGuid, x => x.CompanyPlatformSubscription);
            if (company.CompanyPlatformSubscription is { TrialNotAvailable: true })
            {
                var language = _aspCurrentUserService.GetCurrentUser() == null ? new En() : _aspCurrentUserService.GetCurrentUser().Language;
                throw new ArgumentException(Translate.Get(() => Translations.ApplySubscriptionPlan.Errors.TrialPlanAlreadyUsed, language));
            }
        }
        _companyRwRepo.UpdateData(
            x => x.Guid == companyGuid,
            x =>
            {
                if (subscriptionPlan.IsTrial)
                {
                    if (x.CompanyPlatformSubscription != null)
                        x.CompanyPlatformSubscription.TrialNotAvailable = true;
                }

                if (x.CompanyPlatformSubscription == null)
                {
                    x.CompanyPlatformSubscription = new CompanyPlatformSubscription()
                    {
                        SubscriptionPlanGuid = subscriptionPlan.Guid,
                        SubscriptionPlanName = subscriptionPlan.PlanName,
                        SubscriptionActiveTo = startingFrom
                            .AddMonths(subscriptionPlan.Interval)
                            .AddDays(subscriptionPlan.IntervalDays),
                        TrialNotAvailable = subscriptionPlan.IsTrial
                    };
                }
                else
                {
                    x.CompanyPlatformSubscription.SubscriptionPlanGuid = subscriptionPlan.Guid;
                    x.CompanyPlatformSubscription.SubscriptionPlanName = subscriptionPlan.PlanName;
                    x.CompanyPlatformSubscription.SubscriptionActiveTo = startingFrom
                        .AddMonths(subscriptionPlan.Interval)
                        .AddDays(subscriptionPlan.IntervalDays);
                }
            },
            x => x.CompanyPlatformSubscription);
    }

    public void Revoke(Guid companyGuid)
    {
        _companyRwRepo.UpdateData(
            x => x.Guid == companyGuid,
            x =>
            {
                if (x.CompanyPlatformSubscription != null)
                {
                    x.CompanyPlatformSubscription.SubscriptionActiveTo = DateTime.Now.AddMinutes(-1);
                }
            },
            x => x.CompanyPlatformSubscription);
    }
}