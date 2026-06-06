namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;

public record WashInstructionsListResponse(List<WashInstructionItem> WashInstructions);

public record WashInstructionItem(Guid WashInstructionId, Guid IsotainerTankId, string TankNumber, Guid WashTypeId, string Wash, DateTime InstructedOn);