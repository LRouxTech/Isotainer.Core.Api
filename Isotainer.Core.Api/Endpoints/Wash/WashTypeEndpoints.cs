using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Wash;

public static class WashTypeEndpoints
{
    public static IEndpointRouteBuilder MapWashTypeEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/wash/washtype")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("WashType");
        
        group.MapGet("/", async ([FromServices] IWashTypeService washTypeService) =>
            {
                var result = await washTypeService.GetWashTypes();
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetWashTypes")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        
        group.MapPost("/", async ([FromBody] CreateWashTypeRequest createWashTypeRequest, [FromServices] IWashTypeService washTypeService) =>
            {
                var result = await washTypeService.CreateWashType(createWashTypeRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("CreateWashType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{WashTypeId:guid}", async (Guid WashTypeId, [FromBody] UpdateWashTypeRequest updateWashTypeRequest, [FromServices] IWashTypeService washTypeService) =>
            {
                var result = await washTypeService.UpdateWashType(WashTypeId, updateWashTypeRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("UpdateWashType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{washTypeId:guid}/", async (Guid washTypeId, [FromServices] IWashTypeService washTypeService) =>
            {
                var result = await washTypeService.ArchiveWashType(washTypeId);
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