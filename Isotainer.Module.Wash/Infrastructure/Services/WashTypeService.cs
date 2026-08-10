using Isotainer.Module.Wash.Core.Entities;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using Isotainer.Module.Wash.Infrastructure.Database;
using Isotainer.Module.Wash.Infrastructure.Errors;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Wash.Infrastructure.Services;

public class WashTypeService(IWashDbContextFactory dbContextFactory, IWashTypeValidator washTypeValidator) : IWashTypeService
{
    public async Task<Result<WashTypeResponse>> CreateWashType(CreateWashTypeRequest request, CancellationToken ct = default)
    {
        var validation = washTypeValidator.ValidateCreateWashType(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        if (await washContext.WashTypes.AnyAsync(x => x.Type == request.Type, cancellationToken: ct))
        {
            return WashTypeErrors.NotUnique;
        }

        var newWashType = new WashType
        {
            Type = request.Type,
            Cost = request.Cost,
        }.Create();
        
        await washContext.WashTypes.AddAsync(newWashType, ct);
        await washContext.SaveChangesAsync(ct);
        
        return new WashTypeResponse(newWashType.Id, newWashType.Type, newWashType.Cost);
    }

    public async Task<Result<WashTypeResponse>> UpdateWashType(Guid washTypeId, UpdateWashTypeRequest request, CancellationToken ct = default)
    {
        var validation = washTypeValidator.ValidateUpdateWashType(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        if (await washContext.WashTypes.AnyAsync(x => x.Type == request.Type && x.Id != washTypeId, cancellationToken: ct))
        {
            return WashTypeErrors.NotUnique;
        }
        
        var washType = await washContext.WashTypes.FirstOrDefaultAsync(x => x.Id == washTypeId, ct);

        if (washType == null)
        {
            return WashTypeErrors.NotFound;
        }
        
        washType.Type = request.Type;
        washType.Cost = request.Cost;
        washType.Update();
        
        washContext.WashTypes.Update(washType);
        await washContext.SaveChangesAsync(ct);
        
        return new WashTypeResponse(washType.Id, washType.Type, washType.Cost);
    }
    

    public async Task<Result<PagedList<WashTypeItem>>> GetWashTypes(PagedRequest request, CancellationToken ct = default)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        var query = washContext.WashTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x => x.Type.ToLower().Contains(request.Search.ToLower()));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(x => x.Id)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new WashTypeItem(x.Id, x.Type, x.Cost))
            .ToListAsync(ct);

        return new PagedList<WashTypeItem>(items, totalCount, request.PageIndex, request.PageSize);
    }

    public async Task<Result<bool>> ArchiveWashType(Guid washTypeId, CancellationToken ct = default)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);

        var washType = await washContext.WashTypes.FirstOrDefaultAsync(x => x.Id == washTypeId, ct);

        if (washType == null)
        {
            return WashTypeErrors.NotFound;
        }

        washType.Archive();
        washContext.WashTypes.Update(washType);
        await washContext.SaveChangesAsync(ct);

        return true;
    }
    
    public async Task<Result<int>> GetTotalRecords(CancellationToken ct = default)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        var generalCostCount = await washContext.WashTypes
            .CountAsync(ct);
        
        return generalCostCount;
    }
    
    public async Task<Result<DateTime>> GetLastUpdated(CancellationToken ct = default)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        var lastUpdatedOrNull = await washContext.WashTypes
            .MaxAsync(x => (DateTime?)(x.UpdatedOn > x.CreatedOn ? x.UpdatedOn : x.CreatedOn), cancellationToken: ct);

        var lastUpdated = lastUpdatedOrNull ?? DateTime.MinValue;
        return lastUpdated;
    }
}