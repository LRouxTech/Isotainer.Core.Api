using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.Extensions;
using Isotainer.Core.Api.StatisticModels;
using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Tank;

public static class CompanyEndpoints
{
    public static IEndpointRouteBuilder MapCompanyEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/tank/company")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("Company");
        
        group.MapGet("/", async ([AsParameters] PagedRequest request, [FromServices] ICompanyService companyService) =>
            {
                var result = await companyService.GetCompanyList(request);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetCompanies")
            .RequirePermission(IsotainerPermissions.Tank.ViewCompanies)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", async ([FromBody] CreateCompanyRequest createCompanyRequest, [FromServices] ICompanyService companyService) =>
            {
                var result = await companyService.CreateCompany(createCompanyRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("CreateCompany")
            .RequirePermission(IsotainerPermissions.Tank.CreateCompany)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{CompanyId:guid}", async (Guid companyId, [FromBody] UpdateCompanyRequest createCompanyRequest, [FromServices] ICompanyService companyService) =>
            {
                var result = await companyService.UpdateCompany(companyId, createCompanyRequest);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("UpdateCompany")
            .RequirePermission(IsotainerPermissions.Tank.UpdateCompany)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{CompanyId:guid}/", async (Guid companyId, [FromServices] ICompanyService companyService) =>
            {
                var result = await companyService.ArchiveCompany(companyId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("ArchiveCompany")
            .RequirePermission(IsotainerPermissions.Tank.DeleteCompany)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/stats", async ([FromServices] ICompanyService companyService) =>
            {
                Task<Result<int>> totalRecordsTask = companyService.GetTotalRecords();
                Task<Result<DateTime>> lastUpdatedTask = companyService.GetLastUpdated();

                await Task.WhenAll(totalRecordsTask, lastUpdatedTask);

                var totalRecords = await totalRecordsTask;
                var lastUpdated = await lastUpdatedTask;

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
                    LastUpdated =  lastUpdated.Value.ToRelativeTime(),
                    RecordCount = totalRecords.Value,
                });

            })
            .WithName("GetCompanyStats")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}