using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashInstructionService
{
    Task<Result<WashInstructionResponse>>  CreateWashInstruction(CreateWashInstructionRequest request);
    Task<Result<WashInstructionResponse>>  UpdateWashInstruction(Guid washInstructionId, UpdateWashInstructionRequest request);
    Task<Result<WashInstructionsListResponse>> GetWashInstructions(Guid washTypeId);
    Task<Result<bool>> ArchiveWashInstruction(Guid washInstructionId);
}