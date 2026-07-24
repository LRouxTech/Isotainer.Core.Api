using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.ValidationResult;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Finance;

public static class GeneralCostEndpoints
{
    public static IEndpointRouteBuilder MapGeneralCostEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/finance/generalcost")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("General Costs");

        group.MapGet("/",
                async ([AsParameters] PagedRequest request, [FromServices] IGeneralCostService generalCostService) =>
                {
                    var result = await generalCostService.GetGeneralCosts(request);
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

        group.MapPut("/{Id:guid}",
                async (Guid id,
                    [FromBody] UpdateGeneralCostRequest request,
                    [FromServices] IGeneralCostService generalCostService) =>
                {
                    var result = await generalCostService.UpdateGeneralCost(id,
                        request);
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

        group.MapGet("/stats",
                async ([FromServices] IGeneralCostService generalCostService) =>
                {
                    Task<Result<int>> totalRecordsTask = generalCostService.GetTotalRecords();
                    Task<Result<DateTime>> lastUpdatedTask = generalCostService.GetLastUpdated();

                    await Task.WhenAll(totalRecordsTask,
                        lastUpdatedTask);

                    var totalRecords = totalRecordsTask.Result;
                    var lastUpdated = lastUpdatedTask.Result;

                    if (totalRecords.IsFailure)
                    {
                        return Results.BadRequest(totalRecords.Error);
                    }

                    if (lastUpdated.IsFailure)
                    {
                        return Results.BadRequest(lastUpdated.Error);
                    }

                    return Results.Ok(new CardInformation
                    {
                        LastUpdated = lastUpdated.Value.ToRelativeTime(),
                        RecordCount = totalRecords.Value,
                    });

                })
            .WithName("GetGeneralCostsStats")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }


}