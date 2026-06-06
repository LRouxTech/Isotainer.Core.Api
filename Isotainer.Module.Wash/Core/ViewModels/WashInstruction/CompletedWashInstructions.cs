namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;


public record CompletedWashInstructions(Guid WashInstructionId, Guid IsotainerTankId, string wash, double cost, DateTime washedOn);