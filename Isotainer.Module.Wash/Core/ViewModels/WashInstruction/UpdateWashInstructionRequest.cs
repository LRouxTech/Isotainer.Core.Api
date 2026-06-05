namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;

public record UpdateWashInstructionRequest(Guid IsotainerTankId, Guid WashTypeId, DateTime InstructedOn);