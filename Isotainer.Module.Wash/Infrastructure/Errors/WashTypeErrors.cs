using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Infrastructure.Errors;

public static class WashTypeErrors
{
    public static readonly Error NotFound = new("WashType.NotFound", "Wash type not found.");
    public static readonly Error NegativeCost = new("WashType.NegativeCost", "Wash cost cannot be less than zero.");
    public static readonly Error EmptyType = new("WashType.EmptyType", "Type cannot be empty.");
}