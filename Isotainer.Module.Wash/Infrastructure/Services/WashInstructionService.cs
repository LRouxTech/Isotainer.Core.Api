using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Infrastructure.Services;

public class WashInstructionService : IWashInstructionService
{
    public Result<WashInstructionResponse> CreateWashInstruction(CreateWashInstructionRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<WashInstructionResponse> UpdateWashInstruction(UpdateWashInstructionRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<WashInstructionsListResponse> GetWashInstructions(WashInstructionListRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<bool> ArchiveWashInstruction(ArchiveWashInstructionRequest request)
    {
        throw new NotImplementedException();
    }
}