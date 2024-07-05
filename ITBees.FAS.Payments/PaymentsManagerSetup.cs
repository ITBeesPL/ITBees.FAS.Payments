using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITBees.FAS.Payments;

public class PaymentsManagerSetup : IFasDependencyRegistrationWithGenerics
{
    
    public void RegisterDefaultPaymentProvider<T>(IServiceCollection services, T fasPaymentProcessor) where T : IFasPaymentProcessor
    {
        services.AddScoped(typeof(IFasPaymentProcessor), fasPaymentProcessor.GetType());
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
    }
}