using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Finance;

public static class InvoiceEndpoints
{
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/finance/invoice")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("Invoice");
        
        group.MapGet("/tank/{TankId:guid}/", async (Guid TankId, [FromServices] IInvoiceService generalCostService) =>
            {
                var result = await generalCostService.GetInvoices(TankId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetTankInvoices")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/company/{CompanyId:guid}/", async (Guid companyId, [FromServices] IInvoiceService generalCostService) =>
            {
                var result = await generalCostService.GetInvoices(companyId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetCompanyInvoices")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/tank/{TankId:guid}",
                async (Guid tankId,
                    [FromServices] IInvoiceService generalCostService) =>
                {
                    var result = await generalCostService.GenerateIsotainerTankInvoice(tankId);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);
                })
            .WithName("GenerateTankInvoice")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/company/{CompanyId:guid}",
                async (Guid companyId,
                    [FromServices] IInvoiceService generalCostService) =>
                {
                    var result = await generalCostService.GenerateCompanyInvoice(companyId);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);
                })
            .WithName("GenerateTankInvoice")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}