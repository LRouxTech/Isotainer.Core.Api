using Isotainer.Module.Finance.Core.Interfaces;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using Isotainer.Module.Finance.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Validator;

public class InvoiceValidator : IInvoiceValidator
{
    public Result<bool> ValidateGenerateCompanyInvoice(GenerateCompanyInvoiceRequest request)
    {
        if (request.CompanyId == Guid.Empty)
        {
            return InvoiceErrors.CompanyNotFound;
        }
        
        return true;
    }

    public Result<bool> ValidateGenerateIsotainerTankInvoice(GenerateIsotainerTankInvoiceRequest request)
    {
        if (request.IsotainerTankId == Guid.Empty)
        {
            return TankErrors.TankNotFound;
        }
        
        return true;
    }

    public Result<bool> ValidateInvoiceListRequest(InvoiceListRequest request)
    {
        if (request.CompanyId == Guid.Empty)
        {
            return InvoiceErrors.CompanyNotFound;
        }
        return true;
    }
}