using Isotainer.Module.Finance.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Isotainer.Core.Api.Endpoints.Finance;

public static class InvoiceLineEndpoints
{
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/finance/invoicelines")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("InvoiceLine");
        
        group.MapGet("/{InvoiceId:guid}/", async (Guid invoiceId, [FromServices] IInvoiceLineService invoiceLineService) =>
            {
                var result = await invoiceLineService.GetInvoiceLines(invoiceId);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            })
            .WithName("GetCompanyInvoices")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}