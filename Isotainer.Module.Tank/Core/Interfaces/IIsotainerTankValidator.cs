using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces;

public interface IIsotainerTankValidator
{
    Result<bool> ValidateCreateRequest(CreateIsotainerTankRequest request);
    Result<bool> ValidateUpdateRequest(UpdateIsotainerTankRequest request);
    Result<bool> ValidateChangeIsotainerWashStatus(ChangeIsotainerWashStatus request);
    Result<bool> ValidateUnloadTankRequest(UnloadTankRequest request);
    Result<bool> ValidateArchiveRequest(ArchiveIsotainerRequest request);
}