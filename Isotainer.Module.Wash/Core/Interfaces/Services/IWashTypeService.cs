using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashTypeService
{
    Task<Result<WashTypeResponse>> CreateWashType(CreateWashTypeRequest request);
    Task<Result<WashTypeResponse>> UpdateWashType(UpdateWashTypeRequest request);
    Task<Result<WashTypeListResponse>> ListWashTypes();
    Task<Result<bool>> ArchiveWashType(ArchiveWashTypeRequest request);
}