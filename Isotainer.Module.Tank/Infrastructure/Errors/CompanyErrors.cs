using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Errors;

public static class CompanyErrors
{
    public static readonly Error EmptyName = new("Company.EmptyName", "Company name cannot be empty.");
    public static readonly Error NotFound = new("Company.NotFound", "Company cannot be found.");
}