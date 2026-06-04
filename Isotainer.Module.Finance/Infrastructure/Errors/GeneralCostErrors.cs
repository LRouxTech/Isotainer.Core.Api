using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Errors;

public static class GeneralCostErrors
{
    public static readonly Error InvalidCost = new("GeneralCost.EmptyName", "Cost cannot be less than zero.");
    public static readonly Error NotFound = new("GeneralCost.NotFound", "General Cost Item cannot be found.");
}