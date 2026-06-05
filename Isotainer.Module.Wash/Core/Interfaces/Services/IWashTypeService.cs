using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashTypeService
{
    Result<WashTypeResponse> CreateWashType(CreateWashTypeRequest request);
    Result<WashTypeResponse> UpdateWashType(UpdateWashTypeRequest request);
    Result<WashTypeListResponse> ListWashTypes();
    Result<bool> ArchiveWashType(ArchiveWashTypeRequest request);
}