namespace Isotainer.Core.Api.Endpoints.Finance;

public static class InvoiceLineEndpoints
{
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder endpoints,
        string prefix = "/api/finance/invoicelines")
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags("InvoiceLine");

        return endpoints;
    }
}