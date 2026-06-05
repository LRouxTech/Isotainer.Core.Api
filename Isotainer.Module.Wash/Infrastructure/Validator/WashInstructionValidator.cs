using Isotainer.Module.Wash.Core.Interfaces;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Infrastructure.Validator;

public class WashInstructionValidator : IWashInstructionValidator
{
    public Result<bool> ValidateCreateInstruction(CreateWashInstructionRequest request)
    {
        if (request.IsotainerTankId == Guid.Empty)
        {
            return WashInstructionErrors.TankNotFound;
        }
        
        if (request.WashTypeId == Guid.Empty)
        {
            return WashTypeErrors.NotFound;
        }

        if (request.InstructedOn <= DateTime.UtcNow)
        {
            return WashInstructionErrors.InvalidInstructedOnDate;
        }
        
        return true;
    }

    public Result<bool> ValidateUpdateInstruction(UpdateWashInstructionRequest request)
    {
        if (request.IsotainerTankId == Guid.Empty)
        {
            return WashInstructionErrors.TankNotFound;
        }
        
        if (request.WashTypeId == Guid.Empty)
        {
            return WashTypeErrors.NotFound;
        }

        if (request.InstructedOn <= DateTime.UtcNow)
        {
            return WashInstructionErrors.InvalidInstructedOnDate;
        }
        
        return true;
    }
}