using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Wash;

public static class WashInstructionEndpoints
{
    public static IEndpointRouteBuilder MapWashInstructionEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/wash/instruction")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("WashInstructions");
        
        group.MapGet("/", async ([AsParameters] PagedRequest request, [FromQuery] bool isFinished, [FromServices] IWashInstructionService washInstructionService, [FromServices] IIsotainerTankService tankService, CancellationToken ct) =>
            {
                var result = await washInstructionService.GetWashInstructions(isFinished, null, request, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                var tankIds = result.Value.Items.Select(x => x.IsotainerTankId).ToList();

                var tanks = await tankService.GetIsotainerTanks(tankIds, ct);
                if (tanks.IsFailure)
                {
                    return Results.BadRequest(tanks.Error);
                }

                var washInstructions = result.Value.Items.Select(x => x with { TankNumber = tanks.Value[x.IsotainerTankId] }).ToList();

                return Results.Ok(washInstructions);

            })
            .WithName("GetWashInstructions")
            .RequirePermission(IsotainerPermissions.Wash.ViewWashInstructions)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", async ([FromBody] CreateWashInstructionRequest createWashInstructionRequest, [FromServices] IWashInstructionService washInstructionService, CancellationToken ct) =>
            {
                var result = await washInstructionService.CreateWashInstruction(createWashInstructionRequest, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("CreateWashInstruction")
            .RequirePermission(IsotainerPermissions.Wash.CreateWashInstruction)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{WashInstructionId:guid}", async (Guid washInstructionId, [FromBody] UpdateWashInstructionRequest updateWashInstructionRequest, [FromServices] IWashInstructionService washInstructionService, CancellationToken ct) =>
            {
                var result = await washInstructionService.UpdateWashInstruction(washInstructionId, updateWashInstructionRequest, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("UpdateWashInstruction")
            .RequirePermission(IsotainerPermissions.Wash.UpdateWashInstruction)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{washInstructionId:guid}/", async (Guid washInstructionId, [FromServices] IWashInstructionService washInstructionService, CancellationToken ct) =>
            {
                var result = await washInstructionService.ArchiveWashInstruction(washInstructionId, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("ArchiveWashInstruction")
            .RequirePermission(IsotainerPermissions.Wash.DeleteWashInstruction)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        return endpoints;
    }
}