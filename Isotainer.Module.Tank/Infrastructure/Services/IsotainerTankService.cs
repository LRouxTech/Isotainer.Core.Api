using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.Interfaces.Validators;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using Isotainer.Module.Tank.Helpers.Extensions;
using Isotainer.Module.Tank.Infrastructure.Database;
using Isotainer.Module.Tank.Infrastructure.Errors;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class IsotainerTankService(ITankDbContextFactory dbContextFactory, IIsotainerTankValidator tankValidator) : IIsotainerTankService
{
    public async Task<Result<IsotainerTankResponse>> CreateIsotainerTank(CreateIsotainerTankRequest request, CancellationToken ct)
    {
        var validation = tankValidator.ValidateCreateRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        if (await tankContext.IsotainerTanks.AnyAsync(x => x.TankNumber == request.TankNumber, cancellationToken: ct))
        {
            return IsotainerTankErrors.NotUnique;
        }

        var washStatus = await tankContext.WashStatus.FirstOrDefaultAsync(x => x.Type == WashStatusEnum.New, cancellationToken: ct);
        if (washStatus == null)
        {
            return WashStatusErrors.NotFound;
        }

        var newTank = new IsotainerTank
        {
            TankNumber = request.TankNumber,
            CompanyId = request.CompanyId,
            LoadedOn = DateTime.UtcNow,
            WashStatusId = washStatus.Id
        }.Create();
        
        await tankContext.IsotainerTanks.AddAsync(newTank, ct);
        await tankContext.SaveChangesAsync(ct);
        
        return new IsotainerTankResponse(newTank.Id, newTank.TankNumber, newTank.WashStatusId, newTank.CompanyId, newTank.LoadedOn);
    }

    public async Task<Result<IsotainerTankResponse>> UpdateIsotainerTank(Guid isotainerTankId, UpdateIsotainerTankRequest request, CancellationToken ct)
    {
        var validation = tankValidator.ValidateUpdateRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        if (await tankContext.IsotainerTanks.AnyAsync(x => x.TankNumber == request.TankNumber && x.Id != request.CompanyId, cancellationToken: ct))
        {
            return IsotainerTankErrors.NotUnique;
        }
        
        var tank = await tankContext.IsotainerTanks.FirstOrDefaultAsync(x => x.Id == isotainerTankId, ct);
        
        if (tank == null)
        {
            return IsotainerTankErrors.NotFound;
        }
        
        tank.TankNumber = request.TankNumber;
        tank.CompanyId = request.CompanyId;
        
        tankContext.IsotainerTanks.Update(tank);
        await tankContext.SaveChangesAsync(ct);
        
        return new IsotainerTankResponse(tank.Id, tank.TankNumber, tank.WashStatusId, tank.CompanyId, tank.LoadedOn);
    }

    public async Task<Result<PagedList<IsotainerTankItem>>> GetIsotainerTanks(PagedRequest request, CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        var query = tankContext.IsotainerTanks.AsNoTracking();
        
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x => x.TankNumber.ToLower().Contains(request.Search.ToLower()));
        }

        var totalCount = await query.CountAsync(cancellationToken: ct);

        var items = await query
            .OrderBy(x => x.Id)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new IsotainerTankItem(x.Id, x.TankNumber, x.CompanyId, x.WashStatusId, x.LoadedOn, x.UnloadedOn))
            .ToListAsync(ct);

        return new PagedList<IsotainerTankItem>(items, totalCount, request.PageIndex, request.PageSize);
    }
    
    public async Task<Result<Dictionary<Guid, string>>> GetIsotainerTanks(List<Guid> ids, CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        var tanks = await tankContext.IsotainerTanks
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.TankNumber, cancellationToken: ct);
        
        return tanks;
    }
    
    public async Task<Result<IsotainerTank>> GetIsotainerTankDetails(Guid id, CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        var tank = await tankContext.IsotainerTanks
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);

        if (tank == null)
        {
            return  IsotainerTankErrors.NotFound;
        }
        
        return tank;
    }

    public async Task<Result<bool>> ArchiveIsotainerTank(Guid isotainerTankId, CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);

        var tank = await tankContext.IsotainerTanks.FirstOrDefaultAsync(x => x.Id == isotainerTankId, ct);

        if (tank == null)
        {
            return IsotainerTankErrors.NotFound;
        }

        tank.Archive();
        await tankContext.SaveChangesAsync(ct);

        return true;
    }

    public async Task<Result<IsotainerTankResponse>> ChangeWashStatus(Guid isotainerTankId, ChangeWashStatusRequest request, CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        var tank = await tankContext.IsotainerTanks.FirstOrDefaultAsync(x => x.Id == isotainerTankId, ct);

        if (tank == null)
        {
            return IsotainerTankErrors.NotFound;
        }
        
        tank.WashStatusId = request.WashStatusId;
        tank.Update();
        await tankContext.IsotainerTanks.AddAsync(tank, ct);
        await tankContext.SaveChangesAsync(ct);
        
        return new IsotainerTankResponse(tank.Id, tank.TankNumber, tank.WashStatusId, tank.CompanyId, tank.LoadedOn);
    }

    public async Task<Result<IsotainerTankResponse>> UnloadTank(Guid isotainerTankId, CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        var tank = await tankContext.IsotainerTanks.FirstOrDefaultAsync(x => x.Id == isotainerTankId, ct);

        if (tank == null)
        {
            return IsotainerTankErrors.NotFound;
        }
        
        tank.UnloadedOn = DateTime.UtcNow;
        tank.Update();
        await tankContext.IsotainerTanks.AddAsync(tank, ct);
        await tankContext.SaveChangesAsync(ct);
        
        return new IsotainerTankResponse(tank.Id, tank.TankNumber, tank.WashStatusId, tank.CompanyId, tank.LoadedOn);

    }

    public async Task<Result<int>> GetTotalActiveTanks(CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        return await tankContext.IsotainerTanks.CountAsync(x => x.UnloadedOn == null, cancellationToken: ct);
    }

    public async Task<Result<int>> GetNewInventory(CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        var newStatus = await tankContext.WashStatus.FirstOrDefaultAsync(x => x.Type == WashStatusEnum.New, cancellationToken: ct);
        if (newStatus == null)
        {
            return WashStatusErrors.NotFound;
        }
        
        return await tankContext.IsotainerTanks.CountAsync(x => x.WashStatusId == newStatus.Id, cancellationToken: ct);
    }

    public async Task<Result<string>> GetAverageTurnaroundTime(CancellationToken ct)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        var timePairs = await tankContext.IsotainerTanks
            .Where(x => x.UnloadedOn != null)
            .Select(x => new { x.LoadedOn, UnloadedOn = x.UnloadedOn!.Value })
            .ToListAsync(cancellationToken: ct);

        if (!timePairs.Any())
        {
            return Result<string>.Success("No data");
        }

        var avgTicks = (long)timePairs.Average(x => (x.UnloadedOn - x.LoadedOn).Ticks);
        var avgTimeSpan = TimeSpan.FromTicks(avgTicks);

        return Result<string>.Success(avgTimeSpan.ToReadableDuration());
        
        
    }
}