using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface ICompanyService
{
    Result<CompanyResponse> CreateCompany(CreateCompanyRequest request);
    Result<CompanyResponse> UpdateCompany(UpdateCompanyRequest request);
    Result<CompanyListResponse> GetCompanyList();
    Result<bool> ArchiveCompany(ArchiveCompanyRequest request);
}