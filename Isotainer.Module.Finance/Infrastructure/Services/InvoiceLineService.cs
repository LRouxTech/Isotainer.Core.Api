using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceLineService : IInvoiceLineService
{
    public Result<InvoiceLineListResponse> GetInvoiceLines(InvoiceLineListRequest request)
    {
        throw new NotImplementedException();
    }
}