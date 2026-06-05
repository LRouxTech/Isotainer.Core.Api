using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashInstructionService
{
    Result<WashInstructionResponse>  CreateWashInstruction(CreateWashInstructionRequest request);
    Result<WashInstructionResponse>  UpdateWashInstruction(UpdateWashInstructionRequest request);
    Result<WashInstructionsListResponse> GetWashInstructions(WashInstructionListRequest request);
    Result<bool> ArchiveWashInstruction(ArchiveWashInstructionRequest request);
}