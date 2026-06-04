using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces;

public interface IWashTypeValidator
{
    Result<bool> ValidateCreateWashType(CreateWashTypeRequest request);
    Result<bool> ValidateUpdateWashType(UpdateWashTypeRequest request);
    Result<bool> ValidateArchiveWashType(ArchiveWashTypeRequest request);
}