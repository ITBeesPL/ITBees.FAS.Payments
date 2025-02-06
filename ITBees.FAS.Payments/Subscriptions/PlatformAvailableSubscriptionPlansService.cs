using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Lang;
using ITBees.Interfaces.Repository;
using ITBees.Models.Languages;
using ITBees.Models.Payments;
using ITBees.Translations.Interfaces;
using ITBees.UserManager.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITBees.FAS.Payments.Subscriptions;

class PlatformAvailableSubscriptionPlansService : IPlatformAvailableSubscriptionPlansService
{
    private readonly IWriteOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRwRepo;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoPlan;
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IRuntimeTranslationService _runtimeTranslationService;
    private readonly ILanguageSingletonFactory _languageFactory;

    public PlatformAvailableSubscriptionPlansService(IWriteOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRwRepo,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoPlan,
        IAspCurrentUserService aspCurrentUserService,
        IRuntimeTranslationService runtimeTranslationService,
        ILanguageSingletonFactory languageFactory)
    {
        _platformSubscriptionPlanRwRepo = platformSubscriptionPlanRwRepo;
        _platformSubscriptionPlanRoPlan = platformSubscriptionPlanRoPlan;
        _aspCurrentUserService = aspCurrentUserService;
        _runtimeTranslationService = runtimeTranslationService;
        _languageFactory = languageFactory;
    }
    public PlatformSubscriptionPlanVm CreateNew(PlatformSubscriptionPlanIm selectedSubscriptionPlanIm)
    {
        this.ThrowUnauthorizedExceptionIfUserIsNotPlatoformOperator();
        var result = _platformSubscriptionPlanRwRepo.InsertData(new PlatformSubscriptionPlan()
        {
            Value = selectedSubscriptionPlanIm.Value,
            Created = DateTime.Now,
            CreatedByGuid = _aspCurrentUserService.GetCurrentUser().Guid,
            Expires = selectedSubscriptionPlanIm.Expires,
            Interval = selectedSubscriptionPlanIm.Interval,
            IsActive = selectedSubscriptionPlanIm.IsActive,
            IsOneTimePayment = selectedSubscriptionPlanIm.IsOneTimePayment,
            PlanName = selectedSubscriptionPlanIm.PlanName,
            GroupName = selectedSubscriptionPlanIm.GroupName,
            Title = selectedSubscriptionPlanIm.Title,
            Position = selectedSubscriptionPlanIm.Position,
            MostPopular = selectedSubscriptionPlanIm.MostPopular,
            PlanDescription = selectedSubscriptionPlanIm.PlanDescription,
            ButtonText = selectedSubscriptionPlanIm.ButtonText,
            BadgeText = selectedSubscriptionPlanIm.BadgeText,
            CustomImplementation = selectedSubscriptionPlanIm.CustomImplementation,
            CustomImplementationTypeName =selectedSubscriptionPlanIm.CustomImplementationTypeName
        });

        var resultData = _platformSubscriptionPlanRoPlan.GetData(x => x.Guid == result.Guid, x => x.CreatedBy).First();

        return new PlatformSubscriptionPlanVm(resultData);
    }

    public async IAsyncEnumerable<PlatformSubscriptionPlanVm> GetAllActivePlans(string acceptLanguage)
    {
        var lang = GetCurrentUserLanguage(acceptLanguage);
        var result = _platformSubscriptionPlanRoPlan
            .GetDataQueryable(x => x.IsActive,
                x => x.PlanFeatures, x => x.Language)
            .Include(x => x.PlanFeatures)
            .ThenInclude(x => x.PlatformFeature)
            .OrderBy(x => x.Position)
            .ToList();

        if (result.FirstOrDefault()?.LanguageId == lang.Id)
        {
            foreach (var x in result)
            {
                yield return new PlatformSubscriptionPlanVm(x);
            }

            yield break;
        }

        foreach (var x in result)
        {
            var planVm = new PlatformSubscriptionPlanVm()
            {
                IsOneTimePayment = x.IsOneTimePayment,
                BadgeText = await _runtimeTranslationService.GetTranslation(x.BadgeText, lang, true),
                ButtonText = await _runtimeTranslationService.GetTranslation(x.ButtonText, lang, true),
                Currency = x.Currency,
                Expires = x.Expires,
                GroupName = x.GroupName,
                Guid = x.Guid,
                Interval = x.Interval,
                IntervalDays = x.IntervalDays,
                IsActive = x.IsActive,
                IsTrial = x.IsTrial,
                Language = x.Language.Name,
                Position = x.Position,
                PlanFeatures = new List<PlanFeatureVm>(),
                LanguageId = x.LanguageId,
                MostPopular = x.MostPopular,
                PlanDescription = await _runtimeTranslationService.GetTranslation(x.PlanDescription, lang, true),
                PlanName = await _runtimeTranslationService.GetTranslation(x.PlanName, lang, true),
                Title = await _runtimeTranslationService.GetTranslation(x.Title, lang, true),
                Value = x.Value,
                CustomImplementation =x.CustomImplementation,
                CustomImplementationTypeName = x.CustomImplementationTypeName
            };

            await foreach (var feature in GetPlanFeaturesVms(x.PlanFeatures, lang))
            {
                planVm.PlanFeatures.Add(feature);
            }

            yield return planVm;
        }
    }

    private async IAsyncEnumerable<PlanFeatureVm> GetPlanFeaturesVms(List<PlanFeature> planFeatures, Language lang)
    {
        foreach (var x in planFeatures)
        {
            yield return new PlanFeatureVm()
            {
                Position = x.Position,
                IsActive = x.IsActive,
                Description = await _runtimeTranslationService.GetTranslation(x.Description, lang, true),
                FeatureName = await _runtimeTranslationService.GetTranslation(x.PlatformFeature?.FeatureName, lang, true),
                IsAvailable = x.IsAvailable,
                PlanFeatureId = x.Id,
                PlatformFeatureId = x.PlatformFeatureId
            };
        }
    }

    private Language GetCurrentUserLanguage(string acceptLanguage)
    {
        var currentUser = _aspCurrentUserService.GetCurrentUser();
        if (currentUser != null)
        {
            return currentUser.Language;
        }

        return _languageFactory.Get(acceptLanguage);
    }

    public PlatformSubscriptionPlanVm Update(PlatformSubscriptionPlanUm selectedSubscriptionPlanIm)
    {
        ThrowUnauthorizedExceptionIfUserIsNotPlatoformOperator();

        var result = _platformSubscriptionPlanRwRepo.UpdateData(x => x.Guid == selectedSubscriptionPlanIm.Guid, x =>
        {
            x.Value = selectedSubscriptionPlanIm.Value;
            x.Title = selectedSubscriptionPlanIm.Title;
            x.Expires = selectedSubscriptionPlanIm.Expires;
            x.Interval = selectedSubscriptionPlanIm.Interval;
            x.IsActive = selectedSubscriptionPlanIm.IsActive;
            x.IsOneTimePayment = selectedSubscriptionPlanIm.IsOneTimePayment;
            x.PlanName = selectedSubscriptionPlanIm.PlanName;
            x.GroupName = selectedSubscriptionPlanIm.GroupName;
            x.Title = selectedSubscriptionPlanIm.Title;
            x.Position = selectedSubscriptionPlanIm.Position;
            x.MostPopular = selectedSubscriptionPlanIm.MostPopular;
            x.PlanDescription = selectedSubscriptionPlanIm.PlanDescription;
            x.ButtonText = selectedSubscriptionPlanIm.ButtonText;
            x.BadgeText = selectedSubscriptionPlanIm.BadgeText;
            x.LanguageId = selectedSubscriptionPlanIm.LanguageId;
            x.Currency = selectedSubscriptionPlanIm.Currency;
            x.CustomImplementation = selectedSubscriptionPlanIm.CustomImplementation;
            x.CustomImplementationTypeName = selectedSubscriptionPlanIm.CustomImplementationTypeName;
        }, plan => plan.CreatedBy).First();

        return new PlatformSubscriptionPlanVm(result);

    }

    public void Delete(Guid platformSubscriptionPlanGuid)
    {
        ThrowUnauthorizedExceptionIfUserIsNotPlatoformOperator();

        var result = _platformSubscriptionPlanRwRepo.DeleteData(x => x.Guid == platformSubscriptionPlanGuid);
    }

    private void ThrowUnauthorizedExceptionIfUserIsNotPlatoformOperator()
    {
        if (_aspCurrentUserService.CurrentUserIsPlatformOperator() == false)
        {
            throw new UnauthorizedAccessException("You are not allowed to access this method");
        }
    }

    public PlatformSubscriptionPlanVm Get(Guid selectedSubscriptionPlanGuid)
    {
        var result = _platformSubscriptionPlanRoPlan.GetData(x => x.Guid == selectedSubscriptionPlanGuid).First();

        return new PlatformSubscriptionPlanVm(result);
    }
}