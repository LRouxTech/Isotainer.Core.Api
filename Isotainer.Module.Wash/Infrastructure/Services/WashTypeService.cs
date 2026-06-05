using Isotainer.Module.Wash.Core.Entities;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using Isotainer.Module.Wash.Infrastructure.Database;
using Isotainer.Module.Wash.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Wash.Infrastructure.Services;

public class WashTypeService(IWashDbContextFactory dbContextFactory, IWashTypeValidator washTypeValidator) : IWashTypeService
{
    public async Task<Result<WashTypeResponse>> CreateWashType(CreateWashTypeRequest request)
    {
        var validation = washTypeValidator.ValidateCreateWashType(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        if (await washContext.WashTypes.AnyAsync(x => x.Type == request.Type))
        {
            return WashTypeErrors.NotUnique;
        }

        var newWashType = new WashType
        {
            Type = request.Type,
            Cost = request.Cost,
        }.Create();
        
        await washContext.WashTypes.AddAsync(newWashType);
        await washContext.SaveChangesAsync();
        
        return new WashTypeResponse(newWashType.Id, newWashType.Type, newWashType.Cost);
    }

    public async Task<Result<WashTypeResponse>> UpdateWashType(Guid washTypeId, UpdateWashTypeRequest request)
    {
        var validation = washTypeValidator.ValidateUpdateWashType(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        if (await washContext.WashTypes.AnyAsync(x => x.Type == request.Type && x.Id != washTypeId))
        {
            return WashTypeErrors.NotUnique;
        }
        
        var washType = await washContext.WashTypes.FindAsync(washTypeId);

        if (washType == null)
        {
            return WashTypeErrors.NotFound;
        }
        
        washType.Type = request.Type;
        washType.Cost = request.Cost;
        washType.Update();
        
        washContext.WashTypes.Update(washType);
        await washContext.SaveChangesAsync();
        
        return new WashTypeResponse(washType.Id, washType.Type, washType.Cost);
    }
    

    public async Task<Result<WashTypeListResponse>> GetWashTypes()
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        var washTypes = await washContext.WashTypes
            .Select(x => new WashTypeItem(x.Id, x.Type, x.Cost))
            .ToListAsync();
        
        return new  WashTypeListResponse(washTypes);
    }

    public async Task<Result<bool>> ArchiveWashType(Guid washTypeId)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync();

        var washType = await washContext.WashTypes.FindAsync(washTypeId);

        if (washType == null)
        {
            return WashTypeErrors.NotFound;
        }

        washType.Archive();

        return true;
    }
}