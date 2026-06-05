namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;

public record WashInstructionResponse(Guid WashInstructionId, Guid IsotainerTankId, Guid WashTypeId, DateTime InstructedOn);