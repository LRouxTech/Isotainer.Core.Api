using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IIsotainerTankService
{
    Task<Result<IsotainerTankResponse>> CreateIsotainerTank(CreateIsotainerTankRequest request);
    Task<Result<IsotainerTankResponse>> UpdateIsotainerTank(Guid isotainerTankId, UpdateIsotainerTankRequest request);
    Task<Result<IsotainerTankListResponse>> GetIsotainerTanks();
    Task<Result<bool>> ArchiveIsotainerTank(Guid isotainerTankId);
    Task<Result<IsotainerTankResponse>> ChangeWashStatus(Guid isotainerTankId, ChangeWashStatusRequest request);
    Task<Result<IsotainerTankResponse>> UnloadTank(Guid isotainerTankId);
}