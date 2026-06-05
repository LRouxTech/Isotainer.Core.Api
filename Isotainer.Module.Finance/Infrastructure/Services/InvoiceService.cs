using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceService : IInvoiceService
{
    public Result<CompanyInvoiceResponse> GenerateCompanyInvoice(GenerateCompanyInvoiceRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<IsotainerTankInvoiceResponse> GenerateIsotainerTankInvoice(GenerateIsotainerTankInvoiceRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<InvoiceListResponse> GetInvoices(InvoiceListRequest request)
    {
        throw new NotImplementedException();
    }
}