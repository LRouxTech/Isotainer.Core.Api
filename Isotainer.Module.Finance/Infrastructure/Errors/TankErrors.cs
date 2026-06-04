using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Errors;

public static class TankErrors
{
    public static readonly Error TankNotFound = new("IsotainerTank.NotFound", "Isotainer cannot be found.");
}