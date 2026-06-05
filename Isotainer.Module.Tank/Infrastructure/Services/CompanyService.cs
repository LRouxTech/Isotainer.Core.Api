using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.Interfaces.Validators;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using Isotainer.Module.Tank.Infrastructure.Database;
using Isotainer.Module.Tank.Infrastructure.Errors;
using Isotainer.Module.Tank.Infrastructure.Validator;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class CompanyService(ITankDbContextFactory dbContextFactory, ICompanyValidator companyValidator) : ICompanyService
{
    public async Task<Result<CompanyResponse>> CreateCompany(CreateCompanyRequest request)
    {
        var validation = companyValidator.ValidateCreateRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }

        await using var tankContext = await dbContextFactory.CreateDbContextAsync();
        if (await tankContext.Companies.AnyAsync(x => x.Name == request.Name))
        {
            return CompanyErrors.NotUnique;
        }

        var newCompany = new Company
        {
            Name = request.Name,
        }.Create();
        
        await tankContext.Companies.AddAsync(newCompany);
        await tankContext.SaveChangesAsync();
        
        return new CompanyResponse(newCompany.Id, newCompany.Name);
    }

    public async Task<Result<CompanyResponse>> UpdateCompany(UpdateCompanyRequest request)
    {
        var validation = companyValidator.ValidateUpdateRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }

        await using var tankContext = await dbContextFactory.CreateDbContextAsync();
        if (await tankContext.Companies.AnyAsync(x => x.Name == request.Name && x.Id != request.CompanyId))
        {
            return CompanyErrors.NotUnique;
        }
        
        var company = await tankContext.Companies.FindAsync(request.CompanyId);

        if (company == null)
        {
            return CompanyErrors.NotFound;
        }
        
        company.Name = request.Name;
        
        tankContext.Companies.Update(company);
        await tankContext.SaveChangesAsync();
        
        return new CompanyResponse(company.Id, company.Name);
    }

    public async Task<Result<CompanyListResponse>> GetCompanyList()
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync();
        var companies = await tankContext.Companies
            .Select(x => new CompanyItem(x.Id, x.Name))
            .ToListAsync(); 
        
        return new CompanyListResponse(companies);
    }

    public async Task<Result<bool>> ArchiveCompany(ArchiveCompanyRequest request)
    {
        var validation = companyValidator.ValidateArchiveRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var tankContext = await dbContextFactory.CreateDbContextAsync();

        var company = await tankContext.Companies.FindAsync(request.CompanyId);

        if (company == null)
        {
            return CompanyErrors.NotFound;
        }

        company.Archive();

        return true;
    }
}