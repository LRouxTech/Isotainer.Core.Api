using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Services;

public interface IInvoiceService
{
    Result<CompanyInvoiceResponse>  GenerateCompanyInvoice(GenerateCompanyInvoiceRequest request);
    Result<IsotainerTankInvoiceResponse>  GenerateIsotainerTankInvoice(GenerateIsotainerTankInvoiceRequest request);
    Result<InvoiceListResponse>  GetInvoices(InvoiceListRequest request);
}