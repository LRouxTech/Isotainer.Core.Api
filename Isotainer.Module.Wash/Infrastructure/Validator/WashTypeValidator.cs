using Isotainer.Module.Wash.Core.Interfaces;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using Isotainer.Module.Wash.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Infrastructure.Validator;

public class WashTypeValidator : IWashTypeValidator
{
    public Result<bool> ValidateCreateWashType(CreateWashTypeRequest request)
    {
        if (request.Cost < 0)
        {
            return WashTypeErrors.NegativeCost;
        }

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            return WashTypeErrors.EmptyType;
        }
        
        return true;
    }

    public Result<bool> ValidateUpdateWashType(UpdateWashTypeRequest request)
    {
        if (request.Cost < 0)
        {
            return WashTypeErrors.NegativeCost;
        }

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            return WashTypeErrors.EmptyType;
        }
        
        return true;
    }

    public Result<bool> ValidateArchiveWashType(ArchiveWashTypeRequest request)
    {
        if (request.WashTypeId == Guid.Empty)
        {
            return WashTypeErrors.NotFound;
        }
        
        return true;
    }
}