namespace Isotainer.Module.Wash.Core.ViewModels.WashInstruction;

public record CreateWashInstructionRequest(Guid IsotainerTankId, Guid WashTypeId, DateTime InstructedOn);