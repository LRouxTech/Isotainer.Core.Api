using Isotainer.Module.Tank.Core.Interfaces;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using Isotainer.Module.Tank.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Validator;

public class CompanyValidator : ICompanyValidator
{
    public Result<bool> ValidateCreateRequest(CreateCompanyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return CompanyErrors.EmptyName;
        }

        return true;
    }

    public Result<bool> ValidateUpdateRequest(UpdateCompanyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return CompanyErrors.EmptyName;
        }
        
        if (request.CompanyId == Guid.Empty)
        {
            return CompanyErrors.NotFound;
        }
        
        return true;
    }

    public Result<bool> ValidateArchiveRequest(ArchiveCompanyRequest request)
    {
        if (request.CompanyId == Guid.Empty)
        {
            return CompanyErrors.NotFound;
        }
        
        return true;
    }
}