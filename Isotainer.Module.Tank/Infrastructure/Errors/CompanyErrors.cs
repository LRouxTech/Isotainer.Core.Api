using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Errors;

public static class CompanyErrors
{
    public static readonly Error EmptyName = new("Company.EmptyName", "Company name cannot be empty.");
    public static readonly Error NotFound = new("Company.NotFound", "Company cannot be found.");
    public static readonly Error NotUnique = new("Company.NotUnique", "Company name has already been used.");
}