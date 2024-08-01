using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.FAS.Payments.Services;
using ITBees.FAS.Payments.Subscriptions;
using ITBees.FAS.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITBees.FAS.Payments;

public class PaymentsManagerSetup : IFasDependencyRegistrationWithGenerics
{
    
    public static void RegisterDefaultPaymentProvider<T>(IServiceCollection services) where T : IFasPaymentProcessor
    {
        services.AddScoped(typeof(IFasPaymentProcessor), typeof(T));
    }

    ///// <summary>
    ///// This method is fired by ITBees.FAS.Setup, You don't have to use it.
    ///// </summary>
    ///// <param name="services"></param>
    ///// <param name="configurationRoot"></param>
    //public void Register(IServiceCollection services, IConfigurationRoot configurationRoot)
    //{
    //    services.AddScoped<IFasPaymentManager, FasPaymentManager>();
    //}

    public void Register<TContext, TIdentityUser>(IServiceCollection services, IConfigurationRoot configurationRoot) where TContext : DbContext where TIdentityUser : IdentityUser, new()
    {
        services.AddScoped<IFasPaymentManager, FasPaymentManager>();
        services.AddScoped<IPlatformAvailableSubscriptionPlansService, PlatformAvailableSubscriptionPlansService>();
        services.AddScoped<IPaymentSessionService, PaymentSessionService>();
        services.AddScoped<ISubscriptionPlansService, SelectedSubscriptionPlansService>();
        services.AddScoped<IInvoiceDataService, InvoiceDataService>();
        services.AddScoped<IPlatformFeatureService, PlatformFeatureService>();
        services.AddScoped<IPlanFeatureService, PlanFeatureService>();
        services.AddScoped<IReassignSubscriptionPlansToAvailablePlatformPlans, ReassignSubscriptionPlansToAvailablePlatformPlans>();
    }

    public static void RegisterDbModels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SelectedSubscriptionPlan>().HasKey(x => x.Guid);
        modelBuilder.Entity<SelectedSubscriptionPlan>().HasOne(x => x.PlatformSubscriptionPlan);
        modelBuilder.Entity<PlatformSubscriptionPlan>().HasKey(x => x.Guid);
        modelBuilder.Entity<PlatformSubscriptionPlan>().HasMany(x => x.PlanFeatures).WithOne(x=>x.PlatformSubscriptionPlan);
        modelBuilder.Entity<InvoiceData>().HasKey(x => x.Guid);
        modelBuilder.Entity<PlanFeature>().HasKey(x => x.Id);
        modelBuilder.Entity<PlanFeature>().HasOne(x=>x.PlatformFeature);
        modelBuilder.Entity<PlatformFeature>().HasKey(x => x.Id);
        modelBuilder.Entity<PaymentSession>().HasKey(x => x.Guid);
    }
}