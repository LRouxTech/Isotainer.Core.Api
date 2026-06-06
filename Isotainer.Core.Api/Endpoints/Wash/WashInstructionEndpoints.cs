using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Wash;

public static class WashInstructionEndpoints
{
    public static IEndpointRouteBuilder MapWashInstructionEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/wash/instruction")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("WashInstructions");
        
        group.MapGet("/", async ([FromQuery] bool isFinished, [FromServices] IWashInstructionService washInstructionService, [FromServices] IIsotainerTankService tankService) =>
            {
                var result = await washInstructionService.GetWashInstructions(isFinished, null);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                var tankIds = result.Value.WashInstructions.Select(x => x.IsotainerTankId).ToList();

                var tanks = await tankService.GetIsotainerTanks(tankIds);
                if (tanks.IsFailure)
                {
                    return Results.BadRequest(tanks.Error);
                }

                var washInstructions = result.Value.WashInstructions.Select(x => x with { TankNumber = tanks.Value[x.IsotainerTankId] }).ToList();

                return Results.Ok(washInstructions);

            })
            .WithName("GetWashInstructions")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", async ([FromBody] CreateWashInstructionRequest createWashInstructionRequest, [FromServices] IWashInstructionService washInstructionService) =>
            {
                var result = await washInstructionService.CreateWashInstruction(createWashInstructionRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("CreateWashType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{WashInstructionId:guid}", async (Guid washInstructionId, [FromBody] UpdateWashInstructionRequest updateWashInstructionRequest, [FromServices] IWashInstructionService washInstructionService) =>
            {
                var result = await washInstructionService.UpdateWashInstruction(washInstructionId, updateWashInstructionRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("UpdateWashType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{washInstructionId:guid}/", async (Guid washInstructionId, [FromServices] IWashInstructionService washInstructionService) =>
            {
                var result = await washInstructionService.ArchiveWashInstruction(washInstructionId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("ArchiveWashType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        return endpoints;
    }
}