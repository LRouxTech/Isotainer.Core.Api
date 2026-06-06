using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Errors;

public static class InvoiceErrors
{
    public static readonly Error CompanyNotFound = new("Company.NotFound", "Company cannot be found.");
    public static readonly Error NoLines = new("Invoice.NoLines", "No Line items generated.");
}