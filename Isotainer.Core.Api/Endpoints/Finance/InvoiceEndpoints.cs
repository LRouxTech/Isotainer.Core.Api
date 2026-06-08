using Isotainer.Core.Api.Auth;
using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Infrastructure.Services;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using LRouxTech.Core.Auth.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Finance;

public static class InvoiceEndpoints
{
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/finance/invoice")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("Invoice");
        
        group.MapGet("/tank/{TankId:guid}/", async (Guid TankId, [FromServices] IInvoiceService invoiceService) =>
            {
                var result = await invoiceService.GetInvoices(TankId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetTankInvoices")
            .RequirePermission(IsotainerPermissions.Finance.ViewTankInvoices)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/company/{CompanyId:guid}/", async (Guid companyId, [FromServices] IInvoiceService invoiceService) =>
            {
                var result = await invoiceService.GetInvoices(companyId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetCompanyInvoices")
            .RequirePermission(IsotainerPermissions.Finance.ViewCompanyInvoices)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/tank/{TankId:guid}",
                async (Guid tankId,
                    [FromServices] IInvoiceService invoiceService,
                    [FromServices] IWashInstructionService washInstructionService,
                    [FromServices] IsotainerTankService tankService
                    ) =>
                {
                    var invoiceResult = await invoiceService.GetInvoices(tankId);
                    
                    if (invoiceResult.IsFailure)
                    {
                        return Results.BadRequest(invoiceResult.Error);
                    }

                    var invoiceItems = invoiceResult.Value.InvoiceItems;
                    var maxInvoiceDate = invoiceItems?.Max(x => x.InvoicedOn);
                    
                    // Get the tank details
                    var tankResult = await tankService.GetIsotainerTankDetails(tankId);

                    if (tankResult.IsFailure)
                    {
                        return Results.BadRequest(tankResult.Error);
                    }

                    var tank = tankResult.Value;

                    var lastInvoice = invoiceItems is null or []
                        ? tank.LoadedOn.AddDays(-1)
                        : maxInvoiceDate;

                    var washInstructionResult = await washInstructionService.GetCompletedWashInstructions(tankId, lastInvoice);
                    
                    if (washInstructionResult.IsFailure)
                    {
                        return Results.BadRequest(washInstructionResult.Error);
                    }

                    var washInstructions = washInstructionResult.Value;
                    
                    var result = await invoiceService.GenerateIsotainerTankInvoice(
                        new GenerateIsotainerInvoiceRequest(
                            tank.Id, 
                            tank.CompanyId,
                            tank.LoadedOn, 
                            tank.UnloadedOn,
                            maxInvoiceDate,
                            washInstructions.Select(x => new WashItems(x.wash, x.cost, x.washedOn)).ToList()
                            ));
                    
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);
                })
            .WithName("GenerateTankInvoice")
            .RequirePermission(IsotainerPermissions.Finance.GenerateTankInvoice)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
  

        return endpoints;
    }
}