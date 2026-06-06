using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;

namespace Isotainer.Module.Finance.Core.ViewModels.Invoice;

public record InvoiceListResponse(List<InvoiceItem>? InvoiceItems);

public record InvoiceItem(Guid? IsotainerTankId, Guid? CompanyId, DateTime InvoicedOn, double TotalCost, string? XeroId, List<InvoiceLineItem>? invoiceLineItems);

