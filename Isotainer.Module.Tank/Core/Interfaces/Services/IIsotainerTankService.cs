using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IIsotainerTankService
{
    Result<IsotainerTankResponse> CreateIsotainerTank(CreateIsotainerTankRequest request);
    Result<IsotainerTankResponse> UpdateIsotainerTank(UpdateIsotainerTankRequest request);
    Result<IsotainerTankListResponse> GetIsotainerTanks();
    Result<bool> ArchiveIsotainerTank(ArchiveIsotainerRequest request);
    Result<IsotainerTankResponse> ChangeWashStatus(ChangeWashStatusRequest request);
    Result<IsotainerTankResponse> UnloadTank(UnloadTankRequest request);
}