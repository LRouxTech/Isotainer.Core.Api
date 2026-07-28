using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashInstructionService
{
    Task<Result<WashInstructionResponse>>  CreateWashInstruction(CreateWashInstructionRequest request);
    Task<Result<WashInstructionResponse>>  UpdateWashInstruction(Guid washInstructionId, UpdateWashInstructionRequest request);
    Task<Result<PagedList<WashInstructionItem>>> GetWashInstructions(bool isFinished, Guid? isotainerTankId, PagedRequest request);
    Task<Result<List<CompletedWashInstructions>>> GetCompletedWashInstructions(Guid isotainerTankId, DateTime? from);
    Task<Result<bool>> ArchiveWashInstruction(Guid washInstructionId);
}