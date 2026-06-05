using Isotainer.Module.Tank.Core.Interfaces;
using Isotainer.Module.Tank.Core.Interfaces.Validators;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using Isotainer.Module.Tank.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Validator;

public class IsotainerTankValidator : IIsotainerTankValidator
{
    public Result<bool> ValidateCreateRequest(CreateIsotainerTankRequest request)
    {
        if (request.CompanyId != Guid.Empty)
        {
            return CompanyErrors.NotFound;
        }

        if (string.IsNullOrWhiteSpace(request.TankNumber))
        {
            return IsotainerTankErrors.EmptyTankNumber;
        }
        return true;
    }

    public Result<bool> ValidateUpdateRequest(UpdateIsotainerTankRequest request)
    {
        if (request.CompanyId != Guid.Empty)
        {
            return CompanyErrors.NotFound;
        }

        if (string.IsNullOrWhiteSpace(request.TankNumber))
        {
            return IsotainerTankErrors.EmptyTankNumber;
        }
        
        return true;
    }

    public Result<bool> ValidateChangeIsotainerWashStatus(ChangeWashStatusRequest request)
    {
        if (request.IsotainerTankId != Guid.Empty)
        {
            return IsotainerTankErrors.NotFound;
        }
        
        if (request.WashStatusId != Guid.Empty)
        {
            return WashStatusErrors.NotFound;
        }
        
        return true;
    }

    public Result<bool> ValidateUnloadTankRequest(UnloadTankRequest request)
    {
        if (request.IsotainerTankId != Guid.Empty)
        {
            return IsotainerTankErrors.NotFound;
        }
        
        return true;
    }

    public Result<bool> ValidateArchiveRequest(ArchiveIsotainerRequest request)
    {
        if (request.IsotainerTankId != Guid.Empty)
        {
            return IsotainerTankErrors.NotFound;
        }
        
        return true;
    }
}