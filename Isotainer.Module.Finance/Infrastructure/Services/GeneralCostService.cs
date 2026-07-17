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

    public async Task<Result<GeneralCostListResponse>> GetGeneralCosts()
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var generalCosts = await financeContext.GeneralCosts
            .Select(x => new GeneralCostItem(x.Id, x.CostItem.ToString(), x.Cost))
            .ToListAsync();
        
        return new GeneralCostListResponse(generalCosts);
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