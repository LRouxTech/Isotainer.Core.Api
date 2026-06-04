using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces;

public interface IInvoiceLineValidator
{
    Result<bool> ValidateInvoiceLineListRequest(InvoiceLineListRequest request);
}