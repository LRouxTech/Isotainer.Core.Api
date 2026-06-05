using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IIsotainerTankService
{
    Task<Result<IsotainerTankResponse>> CreateIsotainerTank(CreateIsotainerTankRequest request);
    Task<Result<IsotainerTankResponse>> UpdateIsotainerTank(UpdateIsotainerTankRequest request);
    Task<Result<IsotainerTankListResponse>> GetIsotainerTanks();
    Task<Result<bool>> ArchiveIsotainerTank(ArchiveIsotainerRequest request);
    Task<Result<IsotainerTankResponse>> ChangeWashStatus(ChangeWashStatusRequest request);
    Task<Result<IsotainerTankResponse>> UnloadTank(UnloadTankRequest request);
}