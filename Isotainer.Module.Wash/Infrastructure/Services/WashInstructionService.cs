using Isotainer.Module.Wash.Core.Entities;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Infrastructure.Database;
using Isotainer.Module.Wash.Infrastructure.Errors;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Wash.Infrastructure.Services;

public class WashInstructionService(IWashDbContextFactory dbContextFactory, IWashInstructionValidator washInstructionValidator) : IWashInstructionService
{
    public async Task<Result<WashInstructionResponse>> CreateWashInstruction(CreateWashInstructionRequest request, CancellationToken ct)
    {
        var validation = washInstructionValidator.ValidateCreateInstruction(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);

        var newWashInstruction = new WashInstruction
        {
            IsotainerTankId = request.IsotainerTankId,
            WashTypeId = request.WashTypeId,
            InstructedOn = request.InstructedOn,
        }.Create();
        
        await washContext.WashInstructions.AddAsync(newWashInstruction, ct);
        await washContext.SaveChangesAsync(ct);
        
        return new WashInstructionResponse(newWashInstruction.Id, newWashInstruction.IsotainerTankId, newWashInstruction.WashTypeId, newWashInstruction.InstructedOn);
    }

    public  async Task<Result<WashInstructionResponse>> UpdateWashInstruction(Guid washInstructionId, UpdateWashInstructionRequest request, CancellationToken ct)
    {
        var validation = washInstructionValidator.ValidateUpdateInstruction(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        var washInstruction = await washContext.WashInstructions.FirstOrDefaultAsync(x => x.Id == washInstructionId, cancellationToken: ct);

        if (washInstruction == null)
        {
            return WashInstructionErrors.NotFound;
        }
        
        washInstruction.InstructedOn = request.InstructedOn;
        washInstruction.IsotainerTankId = request.IsotainerTankId;
        washInstruction.WashTypeId = request.WashTypeId;
        washInstruction.Update();
        
        washContext.WashInstructions.Update(washInstruction);
        await washContext.SaveChangesAsync(ct);
        
        return new WashInstructionResponse(washInstruction.Id, washInstruction.IsotainerTankId, washInstruction.WashTypeId, washInstruction.InstructedOn);
    }

    public async Task<Result<WashInstructionResponse>> CompleteWashInstruction(Guid washInstructionId, CancellationToken ct)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        var washInstruction = await washContext.WashInstructions.FirstOrDefaultAsync(x => x.Id == washInstructionId, cancellationToken: ct);
        if (washInstruction == null)
        {
            return WashInstructionErrors.NotFound;
        }
        washInstruction.FinishedOn = DateTime.Now;
        washContext.WashInstructions.Update(washInstruction);
        await washContext.SaveChangesAsync(ct);

        return new WashInstructionResponse(washInstruction.Id, washInstruction.IsotainerTankId, washInstruction.WashTypeId, washInstruction.InstructedOn);
    }

    public  async Task<Result<PagedList<WashInstructionItem>>> GetWashInstructions(bool isFinished, Guid? isotainerTankId, PagedRequest request, CancellationToken ct)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        var query = washContext.WashInstructions.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken: ct);
        
        var items = await query
            .Where(x => isFinished ? x.FinishedOn != null : x.FinishedOn == null)
            .Where(x => isotainerTankId == null  || x.IsotainerTankId == isotainerTankId)
            .OrderBy(x => x.Id)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new WashInstructionItem(x.Id, x.IsotainerTankId, "", x.WashTypeId, x.WashType.Type,  x.InstructedOn ))
            .ToListAsync();

        return new PagedList<WashInstructionItem>(items, totalCount, request.PageIndex, request.PageSize);
    }

    public async Task<Result<List<CompletedWashInstructions>>> GetCompletedWashInstructions(Guid isotainerTankId, DateTime? from, CancellationToken ct)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        var washInstructions = await washContext.WashInstructions
            .Where(x => x.FinishedOn != null && x.FinishedOn > from)
            .Where(x => x.IsotainerTankId == isotainerTankId)
            .Select(x => new CompletedWashInstructions(x.Id, x.IsotainerTankId, x.WashType.Type, x.WashType.Cost, x.FinishedOn.Value ))
            .ToListAsync();

        return washInstructions;
    }

    public  async Task<Result<bool>> ArchiveWashInstruction(Guid washInstructionId, CancellationToken ct)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);

        var washInstruction = await washContext.WashInstructions.FirstOrDefaultAsync(x => x.Id == washInstructionId, cancellationToken: ct);

        if (washInstruction == null)
        {
            return WashTypeErrors.NotFound;
        }

        washInstruction.Archive();

        return true;
    }

    public async Task<Result<int>> GetTotalWashesBooked(CancellationToken ct)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync(ct);
        return await washContext.WashInstructions.CountAsync(x => x.FinishedOn == null, cancellationToken: ct);
    }
}