using ITBees.FAS.Payments.Controllers;
using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Platforms;
using ITBees.Interfaces.Repository;
using ITBees.Mailing.Interfaces;
using ITBees.Models.Companies;
using ITBees.Models.EmailMessages;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

public class ModifiedSubscriptionService : IModifiedSubscriptionService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IPlatformSettingsService _platformSettingsService;
    private readonly IWriteOnlyRepository<Company> _companyWoRepo;
    private readonly IReadOnlyRepository<Company> _companyRoRepo;
    private readonly IEmailSendingService _emailSendingService;

    public ModifiedSubscriptionService(IAspCurrentUserService aspCurrentUserService,
        IPlatformSettingsService platformSettingsService,
        IWriteOnlyRepository<Company> companyWoRepo,
        IReadOnlyRepository<Company> companyRoRepo,
        IEmailSendingService emailSendingService)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _platformSettingsService = platformSettingsService;
        _companyWoRepo = companyWoRepo;
        _companyRoRepo = companyRoRepo;
        _emailSendingService = emailSendingService;
    }

    public ModifiedSubscriptionResultVm Modify(Guid companyGuid, DateTime validTo, string authKey)
    {
        if (string.IsNullOrEmpty(authKey))
        {
            if (_aspCurrentUserService.CurrentUserIsPlatformOperator() == false)
            {
                throw new UnauthorizedAccessException();
            }
        }
        else
        {
            if (_platformSettingsService.GetSetting("platformAuthKey") != authKey)
            {
                throw new UnauthorizedAccessException();
            }
        }

        var company = _companyRoRepo.GetData(x => x.Guid == companyGuid, x => x.CompanyPlatformSubscription).FirstOrDefault();

        try
        {
            _companyWoRepo.UpdateData(x => x.Guid == companyGuid, x =>
            {
                x.CompanyPlatformSubscription.SubscriptionActiveTo = validTo;
            });

            var operatorEmail = _platformSettingsService.GetSetting("PlatformOperatorNotificationEmail");
            string modifiedBy = _aspCurrentUserService.CurrentUserIsPlatformOperator() ? _aspCurrentUserService.GetCurrentSessionUser().CurrentUser.DisplayName : "unknown";
            _emailSendingService.SendEmail(_platformSettingsService.GetPlatformDefaultEmailAccount(), new EmailMessage()
            {
                Subject = $"Modified subscription plan for company : {company.CompanyName}",
                BodyText = "This is notification about change subscription plan by some platform operator. \n" +
                           $"Company : {company.CompanyName} has now subscription plan - {company.CompanyPlatformSubscription.SubscriptionPlanName} \n" +
                           $"active to {company.CompanyPlatformSubscription.SubscriptionActiveTo.ToString()}\n\n" +
                           $"Platform : {_platformSettingsService.GetSetting("SiteUrl")}\n\n" +
                           $"Modified by {modifiedBy}",
                Recipients = operatorEmail
            });

            return new ModifiedSubscriptionResultVm()
            {
                PlanActiveTo = validTo,
                PlanName = company.CompanyPlatformSubscription.SubscriptionPlanName,
                PlanGuid = company.CompanyPlatformSubscription.SubscriptionPlanGuid
            };
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}