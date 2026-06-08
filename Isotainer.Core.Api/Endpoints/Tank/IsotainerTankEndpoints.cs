using Isotainer.Core.Api.Auth;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.Auth.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Tank;

public static class IsotainerTankEndpoints
{
    public static IEndpointRouteBuilder MapIsotainerTankEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/tank/isotainer")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("IsotainerTank");
        
        group.MapGet("/", async ([FromServices] IIsotainerTankService isotainerTankService) =>
            {
                var result = await isotainerTankService.GetIsotainerTanks();
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
        
        group.MapPost("/", async ([FromBody] CreateIsotainerTankRequest createIsotainerTankRequest, [FromServices] IIsotainerTankService isotainerTankService) =>
            {
                var result = await isotainerTankService.CreateIsotainerTank(createIsotainerTankRequest);
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
        
        group.MapPut("/{IsotainerTankId:guid}", async (Guid isotainerTankId, [FromBody] UpdateIsotainerTankRequest updateIsotainerTankRequest, [FromServices] IIsotainerTankService isotainerTankService) =>
            {
                var result = await isotainerTankService.UpdateIsotainerTank(isotainerTankId, updateIsotainerTankRequest);
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
        
        group.MapPut("/{isotainerTankId:guid}/wash-status", async (Guid isotainerTankId, [FromBody] ChangeWashStatusRequest changeWashStatusRequest, [FromServices] IIsotainerTankService isotainerTankService) =>
            {
                var result = await isotainerTankService.ChangeWashStatus(isotainerTankId, changeWashStatusRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            })
            .WithName("ChangeIsotainerTankWashStatus")
            .RequirePermission(IsotainerPermissions.Tank.ChangeIsotainerWashStatus)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);


        group.MapPost("/{isotainerTankId:guid}/unload", async (Guid isotainerTankId, [FromServices] IIsotainerTankService isotainerTankService) =>
            {
                var result = await isotainerTankService.UnloadTank(isotainerTankId);
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
        
        group.MapDelete("/{isotainerTankId:guid}", async (Guid isotainerTankId, [FromServices] IIsotainerTankService isotainerTankService) =>
            {
                var result = await isotainerTankService.ArchiveIsotainerTank(isotainerTankId);
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

        return endpoints;
    }
}