using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces;

public interface ICompanyValidator
{
    Result<bool> ValidateCreateRequest(CreateCompanyRequest request);
    Result<bool> ValidateUpdateRequest(UpdateCompanyRequest request);
    Result<bool> ValidateArchiveRequest(ArchiveCompanyRequest request);
}