using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Services;

public interface IInvoiceService
{
    Task<Result<IsotainerTankInvoiceResponse>> GenerateIsotainerTankInvoice(GenerateIsotainerInvoiceRequest request);
    Task<Result<InvoiceListResponse>> GetInvoices(Guid tankId);
}