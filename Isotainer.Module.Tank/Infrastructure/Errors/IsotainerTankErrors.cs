using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Errors;

public static class IsotainerTankErrors
{
    public static readonly Error NotFound = new("IsotainerTank.NotFound", "Isotainer not found.");
    public static readonly Error EmptyTankNumber = new("IsotainerTank.EmptyTankNumber", "Isotainer tank number cannot be empty.");
    public static readonly Error NotUnique = new("IsotainerTank.NotUnique", "Isotainer tank number not unique.");
}