using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Services;

public interface IInvoiceService
{
    Task<Result<CompanyInvoiceResponse>> GenerateCompanyInvoice(Guid companyId);
    Task<Result<IsotainerTankInvoiceResponse>> GenerateIsotainerTankInvoice(Guid tankId);
    Task<Result<InvoiceListResponse>> GetInvoices(Guid tankId);
}