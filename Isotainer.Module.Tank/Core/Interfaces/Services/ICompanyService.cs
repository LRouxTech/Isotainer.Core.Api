using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface ICompanyService
{
    Task<Result<CompanyResponse>> CreateCompany(CreateCompanyRequest request);
    Task<Result<CompanyResponse>> UpdateCompany(Guid companyId, UpdateCompanyRequest request);
    Task<Result<PagedList<CompanyItem>>> GetCompanyList(PagedRequest request);
    Task<Result<bool>> ArchiveCompany(Guid companyId);
    Task<Result<int>> GetTotalRecords();
    Task<Result<DateTime>> GetLastUpdated();
}