namespace Isotainer.Module.Finance.Core.ViewModels.Invoice;

public record GenerateIsotainerInvoiceRequest(Guid tankId, Guid companyId, DateTime LoadedOn, DateTime? UnloadedOn, DateTime? LastInvoiceDate, List<WashItems> washItems);

public record WashItems(string wash, double cost, DateTime date);