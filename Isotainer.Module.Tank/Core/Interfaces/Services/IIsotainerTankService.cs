using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IIsotainerTankService
{
    Task<Result<IsotainerTankResponse>> CreateIsotainerTank(CreateIsotainerTankRequest request, CancellationToken ct);
    Task<Result<IsotainerTankResponse>> UpdateIsotainerTank(Guid isotainerTankId, UpdateIsotainerTankRequest request, CancellationToken ct);
    Task<Result<PagedList<IsotainerTankItem>>> GetIsotainerTanks(PagedRequest request, CancellationToken ct);
    Task<Result<Dictionary<Guid, string>>> GetIsotainerTanks(List<Guid> ids, CancellationToken ct);
    Task<Result<IsotainerTank>> GetIsotainerTankDetails(Guid id, CancellationToken ct);
    Task<Result<bool>> ArchiveIsotainerTank(Guid isotainerTankId, CancellationToken ct);
    Task<Result<IsotainerTankResponse>> ChangeWashStatus(Guid isotainerTankId, ChangeWashStatusRequest request, CancellationToken ct);
    Task<Result<IsotainerTankResponse>> UnloadTank(Guid isotainerTankId, CancellationToken ct);
    Task<Result<int>> GetTotalActiveTanks(CancellationToken ct);
    Task<Result<int>> GetNewInventory(CancellationToken ct);
    Task<Result<string>> GetAverageTurnaroundTime(CancellationToken ct);
}