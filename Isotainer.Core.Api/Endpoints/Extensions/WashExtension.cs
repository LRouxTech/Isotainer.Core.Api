using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Infrastructure.Services;

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

        return services;
    }
    
    public static IServiceCollection AddValidationWashModule(this IServiceCollection services)
    {
        services.AddScoped<IWashTypeService, WashTypeService>();
        services.AddScoped<IWashInstructionService, WashInstructionService>();

        return services;
    }
    
    public static IServiceCollection AddServiceWashModule(this IServiceCollection services)
    {
        services.AddScoped<IWashInstructionService, WashInstructionService>();
        services.AddScoped<IWashTypeService, WashTypeService>();

        return services;
    }
}