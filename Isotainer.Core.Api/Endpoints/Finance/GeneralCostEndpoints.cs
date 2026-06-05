using Isotainer.Module.Finance.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Finance;

public static class GeneralCostEndpoints
{
    public static IEndpointRouteBuilder MapGeneralCostEndpoints(this IEndpointRouteBuilder endpoints, string prefix = "/api/finance/generalcost")
    {
        var group = endpoints.MapGroup(prefix);

        group.MapGet("/", async ([FromServices] IGeneralCostService generalCostService) =>
            {
                var result = await generalCostService.GetGeneralCosts();
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetGeneralCosts");

        return endpoints;
    }
}