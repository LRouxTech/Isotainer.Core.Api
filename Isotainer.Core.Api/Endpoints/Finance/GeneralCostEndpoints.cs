using Isotainer.Core.Api.Auth;
using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using LRouxTech.Core.Auth.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Finance;

public static class GeneralCostEndpoints
{
    public static IEndpointRouteBuilder MapGeneralCostEndpoints(this IEndpointRouteBuilder endpoints, string prefix = "/api/finance/generalcost")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("General Costs");

        group.MapGet("/", async ([FromServices] IGeneralCostService generalCostService) =>
            {
                var result = await generalCostService.GetGeneralCosts();
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetGeneralCosts")
            .RequirePermission(IsotainerPermissions.Finance.ViewGeneralCosts)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{Id:guid}", async (Guid id, [FromBody] UpdateGeneralCostRequest request, [FromServices] IGeneralCostService generalCostService) =>
            {
                var result = await generalCostService.UpdateGeneralCost(id, request);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            })
            .WithName("UpdateGeneralCost")
            .RequirePermission(IsotainerPermissions.Finance.UpdateGeneralCosts)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
    
    
}