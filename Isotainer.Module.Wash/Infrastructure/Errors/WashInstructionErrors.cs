using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Infrastructure.Errors;

public static class WashInstructionErrors
{
    public static readonly Error NotFound = new("WashInstruction.NotFound", "Wash instruction not found.");
    public static readonly Error TankNotFound = new("IsotainerTank.NotFound", "Tank not found.");
    public static readonly Error InvalidInstructedOnDate = new("WashInstruction.InvalidInstructedOnDate", "Instructions cannot be made in the past.");
}