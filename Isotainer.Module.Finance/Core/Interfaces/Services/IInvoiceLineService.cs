using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Services;

public interface IInvoiceLineService
{
    Task<Result<InvoiceLineListResponse>> GetInvoiceLines(Guid invoiceId);
}