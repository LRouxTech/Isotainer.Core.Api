using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Infrastructure.Services;
using Isotainer.Module.Finance.Infrastructure.Validator;
using Microsoft.AspNetCore.Identity;

namespace Isotainer.Core.Api.Endpoints.Extensions;

public static class FinanceExtension
{
    public static IServiceCollection AddFinanceModule(this IServiceCollection services)
    {
        services.AddValidationFinanceModule();
        services.AddServiceFinanceModule();
        services.AddFinanceContext();

        return services;
    }
    
    public static IServiceCollection AddFinanceContext(this IServiceCollection services)
    {

        return services;
    }
    
    public static IServiceCollection AddValidationFinanceModule(this IServiceCollection services)
    {
        services.AddScoped<IGeneralCostValidator, GeneralCostValidator>();

        return services;
    }
    
    public static IServiceCollection AddServiceFinanceModule(this IServiceCollection services)
    {
        services.AddScoped<IGeneralCostService, GeneralCostService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoiceLineService, InvoiceLineService>();

        return services;
    }
}