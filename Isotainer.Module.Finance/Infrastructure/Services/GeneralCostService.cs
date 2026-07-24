using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using Isotainer.Module.Finance.Infrastructure.Database;
using Isotainer.Module.Finance.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class GeneralCostService(IFinanceDbContextFactory dbContextFactory, IGeneralCostValidator generalCostValidator) : IGeneralCostService
{
    public async Task<Result<GeneralCostUpdateResponse>> UpdateGeneralCost(Guid id, UpdateGeneralCostRequest request)
    {
        var validation =  generalCostValidator.ValidateUpdateGeneralCost(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var generalCost = await financeContext.GeneralCosts.FirstOrDefaultAsync(x => x.Id == id);
        if (generalCost == null)
        {
            return GeneralCostErrors.NotFound;
        }
        
        generalCost.Cost = request.Cost;
        
        financeContext.GeneralCosts.Update(generalCost);
        await financeContext.SaveChangesAsync();
        
        return new GeneralCostUpdateResponse(generalCost.Id, generalCost.Cost);
    }

    public async Task<Result<PagedList<GeneralCostItem>>> GetGeneralCosts(PagedRequest request)
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var query = financeContext.GeneralCosts.AsNoTracking();
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderBy(x => x.Id)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new GeneralCostItem(x.Id, x.CostItem.ToString(), x.Cost))
            .ToListAsync();

        return new PagedList<GeneralCostItem>(items, totalCount, request.PageIndex, request.PageSize);
    }
    
    public async Task<Result<int>> GetTotalRecords()
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var generalCostCount = await financeContext.GeneralCosts
            .CountAsync();
        
        return generalCostCount;
    }
    
    public async Task<Result<DateTime>> GetLastUpdated()
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var lastUpdatedOrNull = await financeContext.GeneralCosts
            .MaxAsync(x => (DateTime?)(x.UpdatedOn > x.CreatedOn ? x.UpdatedOn : x.CreatedOn));

        var lastUpdated = lastUpdatedOrNull ?? DateTime.MinValue;
        return lastUpdated;
    }
}