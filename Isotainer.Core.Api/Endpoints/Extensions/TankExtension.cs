using Isotainer.Module.Tank.Core.Cache;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.Interfaces.Validators;
using Isotainer.Module.Tank.Infrastructure.Database;
using Isotainer.Module.Tank.Infrastructure.Services;
using Isotainer.Module.Tank.Infrastructure.Validator;
using Microsoft.Extensions.Caching.Hybrid;

namespace Isotainer.Core.Api.Endpoints.Extensions;

public static class TankExtension
{
    public static IServiceCollection AddTankModule(this IServiceCollection services)
    {
        services.AddValidationTankModule();
        services.AddTankContext();
        services.AddServiceTankModule();

        return services;
    }
    
    public static IServiceCollection AddTankContext(this IServiceCollection services)
    {
        services.AddSingleton<ITankDbContextFactory, TankDbContextFactory>();

        return services;
    }
    
    public static IServiceCollection AddValidationTankModule(this IServiceCollection services)
    {
        services.AddScoped<ICompanyValidator, CompanyValidator>();
        services.AddScoped<IIsotainerTankValidator, IsotainerTankValidator>();

        return services;
    }
    
    public static IServiceCollection AddServiceTankModule(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IIsotainerTankService, IsotainerTankService>();
        services.AddScoped<WashStatusService>();
        services.AddScoped<IWashStatusService>(provider => 
            new CachedWashStatusService(
                provider.GetRequiredService<WashStatusService>(),
                provider.GetRequiredService<HybridCache>(),
                provider.GetRequiredService<ILogger<CachedWashStatusService>>()
            ));

        return services;
    }
}