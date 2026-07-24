using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IIsotainerTankService
{
    Task<Result<IsotainerTankResponse>> CreateIsotainerTank(CreateIsotainerTankRequest request);
    Task<Result<IsotainerTankResponse>> UpdateIsotainerTank(Guid isotainerTankId, UpdateIsotainerTankRequest request);
    Task<Result<PagedList<IsotainerTankItem>>> GetIsotainerTanks(PagedRequest request);
    Task<Result<Dictionary<Guid, string>>> GetIsotainerTanks(List<Guid> ids);
    Task<Result<IsotainerTank>> GetIsotainerTankDetails(Guid id);
    Task<Result<bool>> ArchiveIsotainerTank(Guid isotainerTankId);
    Task<Result<IsotainerTankResponse>> ChangeWashStatus(Guid isotainerTankId, ChangeWashStatusRequest request);
    Task<Result<IsotainerTankResponse>> UnloadTank(Guid isotainerTankId);
}