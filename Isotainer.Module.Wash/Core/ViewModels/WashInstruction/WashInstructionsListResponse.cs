namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;

public record WashInstructionsListResponse(List<WashInstructionItem> WashInstructions);

public class WashInstructionItem(Guid WashInstructionId, Guid IsotainerTankId, Guid WashTypeId, DateTime InstructedOn);