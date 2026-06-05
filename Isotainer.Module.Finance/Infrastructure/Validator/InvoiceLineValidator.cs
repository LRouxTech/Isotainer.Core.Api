using Isotainer.Module.Finance.Core.Interfaces;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using Isotainer.Module.Finance.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Validator;

public class InvoiceLineValidator : IInvoiceLineValidator
{
    public Result<bool> ValidateInvoiceLineListRequest(InvoiceLineListRequest request)
    {
        if (request.IsotainerTankId == Guid.Empty)
        {
            return TankErrors.TankNotFound;
        }
        
        return true;
    }
}