using Isotainer.Module.Wash.Core.Cache;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Infrastructure.Database;
using Isotainer.Module.Wash.Infrastructure.Services;
using Isotainer.Module.Wash.Infrastructure.Validator;
using Microsoft.Extensions.Caching.Hybrid;

namespace Isotainer.Core.Api.Endpoints.Extensions;

public static class WashExtension
{
    public static IServiceCollection AddWashModule(this IServiceCollection services)
    {
        services.AddValidationWashModule();
        services.AddServiceWashModule();
        services.AddWashContext();

        return services;
    }
    
    public static IServiceCollection AddWashContext(this IServiceCollection services)
    {
        services.AddSingleton<IWashDbContextFactory, WashDbContextFactory>();

        return services;
    }
    
    public static IServiceCollection AddValidationWashModule(this IServiceCollection services)
    {
        services.AddScoped<IWashTypeValidator, WashTypeValidator>();
        services.AddScoped<IWashInstructionValidator, WashInstructionValidator>();

        return services;
    }
    
    public static IServiceCollection AddServiceWashModule(this IServiceCollection services)
    {
        services.AddScoped<WashInstructionService>();
        services.AddScoped<IWashInstructionService>(provider => 
            new CachedWashInstructionService(
                provider.GetRequiredService<WashInstructionService>(),
                provider.GetRequiredService<HybridCache>(),
                provider.GetRequiredService<ILogger<CachedWashInstructionService>>()
            ));
        
        services.AddScoped<WashTypeService>();
        services.AddScoped<IWashTypeService>(provider => 
            new CachedWashTypeService(
                provider.GetRequiredService<WashTypeService>(),
                provider.GetRequiredService<HybridCache>(),
                provider.GetRequiredService<ILogger<CachedWashTypeService>>()
            ));


        return services;
    }
}