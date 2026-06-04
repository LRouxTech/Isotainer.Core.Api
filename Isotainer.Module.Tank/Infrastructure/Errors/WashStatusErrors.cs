using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Errors;

public static class WashStatusErrors
{
    public static readonly Error NotFound = new("WashStatus.NotFound", "Wash status not found.");
}