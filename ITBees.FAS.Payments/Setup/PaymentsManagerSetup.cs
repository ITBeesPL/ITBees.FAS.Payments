﻿using ITBees.FAS.Payments.Controllers.Operator;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.FAS.Payments.Services;
using ITBees.FAS.Payments.Services.Operator;
using ITBees.FAS.Payments.Subscriptions;
using ITBees.FAS.Setup;
using ITBees.Interfaces.Lang;
using ITBees.Models.Companies;
using ITBees.Models.Payments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITBees.FAS.Payments.Setup;

public class PaymentsManagerSetup : IFasDependencyRegistrationWithGenerics
{
    public static void RegisterDefaultPaymentProvider<T>(IServiceCollection services) where T : IFasPaymentProcessor
    {
        services.AddScoped(typeof(IFasPaymentProcessor), typeof(T));
    }

    public void Register<TContext, TIdentityUser>(IServiceCollection services, IConfigurationRoot configurationRoot)
        where TContext : DbContext where TIdentityUser : IdentityUser<Guid>, new()
    {
        services.AddScoped<IPaymentSummaryService, PaymentSummaryService>();
        services.AddScoped<IAccessChecker, AccessChecker>();
        services.AddScoped<IPlatformSubscriptionUsageService, PlatformSubscriptionUsageService>();
        services.AddScoped<IModifiedSubscriptionService, ModifiedSubscriptionService>();
        services.AddScoped<IPlatformAvailableSubscriptionPlansService, PlatformAvailableSubscriptionPlansService>();
        services.AddScoped<IPaymentSessionService, PaymentSessionService>();
        services.AddScoped<ISubscriptionPlansService, SelectedSubscriptionPlansService>();
        services.AddScoped<IInvoiceDataService, InvoiceDataService>();
        services.AddScoped<IPlatformFeatureService, PlatformFeatureService>();
        services.AddScoped<IPlanFeatureService, PlanFeatureService>();
        services
            .AddScoped<IReassignSubscriptionPlansToAvailablePlatformPlans,
                ReassignSubscriptionPlansToAvailablePlatformPlans>();
        services.AddScoped<IPaymentSubscriptionService, PaymentSubscriptionService>();
        services.AddScoped<IPaymentSessionCreator, PaymentSessionCreator>();
        services.AddScoped<IPaymentDbLoggerService, PaymentDbLoggerService>();
        services.AddScoped<IApplySubscriptionPlanToCompanyService, ApplySubscriptionPlanToCompanyService>();
        services
            .AddScoped<IApplySubscriptionPlanAsPlatformOperatorService,
                ApplySubscriptionPlanAsPlatformOperatorService>();
        services.AddScoped<IAppleInAppPurchaseService, AppleInAppPurchaseService>();
        services.AddScoped<IPaymentServiceInfo, PaymentServiceInfo>();
        if (services.Any(descriptor =>
                descriptor.ServiceType == typeof(ILanguageSingletonFactory)) == false)
        {
            throw new Exception(
                "You must implement and register ILanguageFactory interface for proper work fas payment module");
        };
    }

    public static void RegisterDbModels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SelectedSubscriptionPlan>().HasKey(x => x.Guid);
        modelBuilder.Entity<SelectedSubscriptionPlan>().HasOne(x => x.PlatformSubscriptionPlan);
        modelBuilder.Entity<PlatformSubscriptionPlan>().HasKey(x => x.Guid);
        modelBuilder.Entity<PlatformSubscriptionPlan>().HasMany(x => x.PlanFeatures)
            .WithOne(x => x.PlatformSubscriptionPlan);
        modelBuilder.Entity<InvoiceData>().HasKey(x => x.Guid);
        modelBuilder.Entity<PlanFeature>().HasKey(x => x.Id);
        modelBuilder.Entity<PlanFeature>().HasOne(x => x.PlatformFeature);
        modelBuilder.Entity<PlatformFeature>().HasKey(x => x.Id);
        modelBuilder.Entity<PaymentSession>().HasKey(x => x.Guid);
        modelBuilder.Entity<PaymentOperatorLog>().HasKey(x => x.Id);
        modelBuilder.Entity<OrderPack>().HasKey(x => x.Guid);
        modelBuilder.Entity<OrderPack>().HasMany(x => x.OrderElements).WithOne(x => x.OrderPack);
        modelBuilder.Entity<OrderElement>().HasKey(x => x.Id);
    }
}