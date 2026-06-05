using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class CompanyService : ICompanyService
{
    public Result<CompanyResponse> CreateCompany(CreateCompanyRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<CompanyResponse> UpdateCompany(UpdateCompanyRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<CompanyListResponse> GetCompanyList()
    {
        throw new NotImplementedException();
    }

    public Result<bool> ArchiveCompany(ArchiveCompanyRequest request)
    {
        throw new NotImplementedException();
    }
}