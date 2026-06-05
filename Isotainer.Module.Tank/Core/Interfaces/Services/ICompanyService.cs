using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface ICompanyService
{
    Task<Result<CompanyResponse>> CreateCompany(CreateCompanyRequest request);
    Task<Result<CompanyResponse>> UpdateCompany(UpdateCompanyRequest request);
    Task<Result<CompanyListResponse>> GetCompanyList();
    Task<Result<bool>> ArchiveCompany(ArchiveCompanyRequest request);
}