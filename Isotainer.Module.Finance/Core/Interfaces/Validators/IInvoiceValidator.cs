using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Validators;

public interface IInvoiceValidator
{
    Result<bool> ValidateGenerateCompanyInvoice(GenerateCompanyInvoiceRequest request);
    Result<bool> ValidateGenerateIsotainerTankInvoice(GenerateIsotainerTankInvoiceRequest request);
    Result<bool> ValidateInvoiceListRequest(InvoiceListRequest request);
}