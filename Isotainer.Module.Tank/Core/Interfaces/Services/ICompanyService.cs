using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface ICompanyService
{
    Task<Result<CompanyResponse>> CreateCompany(CreateCompanyRequest request, CancellationToken ct);
    Task<Result<CompanyResponse>> UpdateCompany(Guid companyId, UpdateCompanyRequest request, CancellationToken ct);
    Task<Result<PagedList<CompanyItem>>> GetCompanyList(PagedRequest request, CancellationToken ct);
    Task<Result<bool>> ArchiveCompany(Guid companyId, CancellationToken ct);
    Task<Result<int>> GetTotalRecords(CancellationToken ct);
    Task<Result<DateTime>> GetLastUpdated(CancellationToken ct);
}