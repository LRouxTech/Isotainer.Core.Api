using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Services;

public interface IInvoiceService
{
    Task<Result<CompanyInvoiceResponse>> GenerateCompanyInvoice(GenerateCompanyInvoiceRequest request);
    Task<Result<IsotainerTankInvoiceResponse>> GenerateIsotainerTankInvoice(GenerateIsotainerTankInvoiceRequest request);
    Task<Result<InvoiceListResponse>> GetInvoices(InvoiceListRequest request);
}