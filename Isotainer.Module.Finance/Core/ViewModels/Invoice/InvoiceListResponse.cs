using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;

namespace Isotainer.Module.Finance.Core.ViewModels.Invoice;

public record InvoiceListResponse(List<InvoiceItem>? InvoiceItems);

public class InvoiceItem(Guid IsotainerTankId, DateTime InvoicedOn, double TotalCost, string? XeroId, List<InvoiceLineItem>? invoiceLineItems);

