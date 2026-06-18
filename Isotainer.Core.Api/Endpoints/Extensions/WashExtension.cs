using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Infrastructure.Database;
using Isotainer.Module.Wash.Infrastructure.Services;
using Isotainer.Module.Wash.Infrastructure.Validator;

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
        services.AddScoped<IWashInstructionService, WashInstructionService>();
        services.AddScoped<IWashTypeService, WashTypeService>();

        return services;
    }
}