using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Tank;

public static class WashStatusEndpoints
{
     public static IEndpointRouteBuilder MapWashStatusEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/tank/washstatus")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("WashStatus");
        
        group.MapGet("/", async ([AsParameters] PagedRequest request, [FromServices] IWashStatusService washStatusService) =>
            {
                var result = await washStatusService.GetWashStatuses(request);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetWashStatuses")
            .RequirePermission(IsotainerPermissions.Tank.ViewWashStatuses)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        return endpoints;
    }
}