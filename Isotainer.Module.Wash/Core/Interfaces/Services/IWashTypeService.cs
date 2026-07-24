using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashTypeService
{
    Task<Result<WashTypeResponse>> CreateWashType(CreateWashTypeRequest request);
    Task<Result<WashTypeResponse>> UpdateWashType(Guid washTypeId, UpdateWashTypeRequest request);
    Task<Result<PagedList<WashTypeItem>>> GetWashTypes(PagedRequest request);
    Task<Result<bool>> ArchiveWashType(Guid washTypeId);
    Task<Result<int>> GetTotalRecords();
    Task<Result<DateTime>> GetLastUpdated();
}