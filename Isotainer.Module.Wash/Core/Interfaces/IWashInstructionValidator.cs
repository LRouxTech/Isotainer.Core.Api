using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces;

public interface IWashInstructionValidator
{
    Result<bool> ValidateCreateInstruction(CreateWashInstructionRequest request);
    Result<bool> ValidateUpdateInstruction(UpdateWashInstructionRequest request);
    Result<bool> ValidateArchiveInstruction(ArchiveWashInstructionRequest request);
}