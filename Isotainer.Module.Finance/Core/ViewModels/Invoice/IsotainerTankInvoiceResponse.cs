namespace Isotainer.Module.Finance.Core.ViewModels.Invoice;

public record IsotainerTankInvoiceResponse(Guid tankId, Guid companyId, DateTime invoiceDate, double totalCost, List<WashItems> washItems);