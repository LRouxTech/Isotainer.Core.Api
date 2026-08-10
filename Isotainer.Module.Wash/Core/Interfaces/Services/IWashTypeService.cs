using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashTypeService
{
    Task<Result<WashTypeResponse>> CreateWashType(CreateWashTypeRequest request, CancellationToken ct = default);
    Task<Result<WashTypeResponse>> UpdateWashType(Guid washTypeId, UpdateWashTypeRequest request, CancellationToken ct = default);
    Task<Result<PagedList<WashTypeItem>>> GetWashTypes(PagedRequest request, CancellationToken ct = default);
    Task<Result<bool>> ArchiveWashType(Guid washTypeId, CancellationToken ct = default);
    Task<Result<int>> GetTotalRecords(CancellationToken ct = default);
    Task<Result<DateTime>> GetLastUpdated(CancellationToken ct = default);
}