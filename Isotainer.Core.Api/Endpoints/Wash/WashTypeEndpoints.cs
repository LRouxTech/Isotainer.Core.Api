using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.Extensions;
using Isotainer.Core.Api.StatisticModels;
using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Wash;

public static class WashTypeEndpoints
{
    public static IEndpointRouteBuilder MapWashTypeEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/wash/washtype")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("WashType");

        group.MapGet("/",
                async ([AsParameters] PagedRequest request, [FromServices] IWashTypeService washTypeService) =>
                {
                    var result = await washTypeService.GetWashTypes(request);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);

                })
            .WithName("GetWashTypes")
            .RequirePermission(IsotainerPermissions.Wash.ViewWashTypes)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);


        group.MapPost("/",
                async ([FromBody] CreateWashTypeRequest createWashTypeRequest,
                    [FromServices] IWashTypeService washTypeService) =>
                {
                    var result = await washTypeService.CreateWashType(createWashTypeRequest);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);

                })
            .WithName("CreateWashType")
            .RequirePermission(IsotainerPermissions.Wash.CreateWashType)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{WashTypeId:guid}",
                async (Guid WashTypeId,
                    [FromBody] UpdateWashTypeRequest updateWashTypeRequest,
                    [FromServices] IWashTypeService washTypeService) =>
                {
                    var result = await washTypeService.UpdateWashType(WashTypeId,
                        updateWashTypeRequest);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);

                })
            .WithName("UpdateWashType")
            .RequirePermission(IsotainerPermissions.Wash.UpdateWashType)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{washTypeId:guid}/",
                async (Guid washTypeId,
                    [FromServices] IWashTypeService washTypeService) =>
                {
                    var result = await washTypeService.ArchiveWashType(washTypeId);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);

                })
            .WithName("ArchiveWashType")
            .RequirePermission(IsotainerPermissions.Wash.DeleteWashType)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/stats",
                async ([FromServices] IWashTypeService washTypeService) =>
                {
                    Task<Result<int>> totalRecordsTask = washTypeService.GetTotalRecords();
                    Task<Result<DateTime>> lastUpdatedTask = washTypeService.GetLastUpdated();

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
            .WithName("GetWashTypeStats")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}