namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;

public record WashInstructionsListResponse(List<WashInstructionItem> WashInstructions);

public class WashInstructionItem(Guid WashInstructionId, string Tanknumber, Guid WashTypeId, DateTime InstructedOn);