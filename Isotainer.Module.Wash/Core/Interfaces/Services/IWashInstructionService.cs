using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Core.Interfaces.Services;

public interface IWashInstructionService
{
    Task<Result<WashInstructionResponse>>  CreateWashInstruction(CreateWashInstructionRequest request, CancellationToken ct);
    Task<Result<WashInstructionResponse>>  UpdateWashInstruction(Guid washInstructionId, UpdateWashInstructionRequest request, CancellationToken ct);
    Task<Result<PagedList<WashInstructionItem>>> GetWashInstructions(bool isFinished, Guid? isotainerTankId, PagedRequest request, CancellationToken ct);
    Task<Result<List<CompletedWashInstructions>>> GetCompletedWashInstructions(Guid isotainerTankId, DateTime? from, CancellationToken ct);
    Task<Result<bool>> ArchiveWashInstruction(Guid washInstructionId, CancellationToken ct);
    Task<Result<int>> GetTotalWashesBooked(CancellationToken ct);
}