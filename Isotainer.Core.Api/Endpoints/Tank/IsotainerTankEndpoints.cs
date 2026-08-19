using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.StatisticModels;
using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Tank;

public static class IsotainerTankEndpoints
{
    public static IEndpointRouteBuilder MapIsotainerTankEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/tank/isotainer")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("IsotainerTank");
        
        group.MapGet("/", async ([AsParameters] PagedRequest request, [FromServices] IIsotainerTankService isotainerTankService, CancellationToken ct) =>
            {
                var result = await isotainerTankService.GetIsotainerTanks(request, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetIsotainerTanks")
            .RequirePermission(IsotainerPermissions.Tank.ViewIsotainers)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", async ([FromBody] CreateIsotainerTankRequest createIsotainerTankRequest, [FromServices] IIsotainerTankService isotainerTankService, CancellationToken ct) =>
            {
                var result = await isotainerTankService.CreateIsotainerTank(createIsotainerTankRequest, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("CreateIsotainerTank")
            .RequirePermission(IsotainerPermissions.Tank.CreateIsotainer)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{IsotainerTankId:guid}", async (Guid isotainerTankId, [FromBody] UpdateIsotainerTankRequest updateIsotainerTankRequest, [FromServices] IIsotainerTankService isotainerTankService, CancellationToken ct) =>
            {
                var result = await isotainerTankService.UpdateIsotainerTank(isotainerTankId, updateIsotainerTankRequest, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("UpdateIsotainerTank")
            .RequirePermission(IsotainerPermissions.Tank.UpdateIsotainer)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{isotainerTankId:guid}/complete", async (Guid isotainerTankId, [FromBody] ChangeWashStatusRequest changeWashStatusRequest, [FromServices] IIsotainerTankService isotainerTankService, [FromServices] IWashInstructionService washInstructionService, CancellationToken ct) =>
            {
                var result = await isotainerTankService.ChangeWashStatus(isotainerTankId, changeWashStatusRequest, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                var completeWash =
                    await washInstructionService.CompleteWashInstruction(changeWashStatusRequest.WashInstructionId, ct);
                if (completeWash.IsFailure)
                {
                    return Results.BadRequest(completeWash.Error);
                }

                return Results.Ok(result.Value);
            })
            .WithName("ChangeIsotainerTankWashStatus")
            .RequirePermission(IsotainerPermissions.Tank.ChangeIsotainerWashStatus)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);


        group.MapPost("/{isotainerTankId:guid}/unload", async (Guid isotainerTankId, [FromServices] IIsotainerTankService isotainerTankService, CancellationToken ct) =>
            {
                var result = await isotainerTankService.UnloadTank(isotainerTankId, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            })
            .WithName("UnloadIsotainerTank")
            .RequirePermission(IsotainerPermissions.Tank.UnloadIsotainer)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{isotainerTankId:guid}", async (Guid isotainerTankId, [FromServices] IIsotainerTankService isotainerTankService, CancellationToken ct) =>
            {
                var result = await isotainerTankService.ArchiveIsotainerTank(isotainerTankId, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("ArchiveIsotainerTank")
            .RequirePermission(IsotainerPermissions.Tank.DeleteIsotainer)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/stats", async ([FromServices] IIsotainerTankService isotainerTankService, IWashInstructionService washInstructionService, CancellationToken ct) =>
            {
                Task<Result<int>> totalActiveTanksTask = isotainerTankService.GetTotalActiveTanks(ct);
                Task<Result<int>> totalNewInventoryTask = isotainerTankService.GetNewInventory(ct);
                Task<Result<int>> totalWashesBookedTask = washInstructionService.GetTotalWashesBooked(ct);
                Task<Result<string>> averageTurnaroundTimeTask = isotainerTankService.GetAverageTurnaroundTime(ct);
                
                await Task.WhenAll(totalActiveTanksTask, totalNewInventoryTask, totalWashesBookedTask, averageTurnaroundTimeTask);

                var totalActiveTanks = await totalActiveTanksTask;
                var totalNewInventory = await totalNewInventoryTask;
                var totalWashesBooked = await totalWashesBookedTask;
                var averageTurnaroundTime = await averageTurnaroundTimeTask;

                if (totalActiveTanks.IsFailure)
                {
                    return Results.BadRequest(totalActiveTanks.Error);
                }

                if (totalNewInventory.IsFailure)
                {
                    return Results.BadRequest(totalNewInventory.Error);
                }
                if (totalWashesBooked.IsFailure)
                {
                    return Results.BadRequest(totalWashesBooked.Error);
                }

                if (averageTurnaroundTime.IsFailure)
                {
                    return Results.BadRequest(averageTurnaroundTime.Error);
                }

                return Results.Ok(new TankStatisticInformation
                {
                    TotalActiveTanks = totalActiveTanks.Value,
                    TotalNewInventory = totalNewInventory.Value,
                    TotalWashesBooked = totalWashesBooked.Value,
                    AverageTurnaroundTime = averageTurnaroundTime.Value,
                });

            })
            .WithName("GetIsotainerTanksStats")
            .RequirePermission(IsotainerPermissions.Tank.ViewIsotainers)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}