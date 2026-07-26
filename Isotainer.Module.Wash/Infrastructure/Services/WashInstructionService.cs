using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Wash.Core.Entities;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.Interfaces.Validators;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Infrastructure.Database;
using Isotainer.Module.Wash.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Wash.Infrastructure.Services;

public class WashInstructionService(IWashDbContextFactory dbContextFactory, IWashInstructionValidator washInstructionValidator) : IWashInstructionService
{
    public async Task<Result<WashInstructionResponse>> CreateWashInstruction(CreateWashInstructionRequest request)
    {
        var validation = washInstructionValidator.ValidateCreateInstruction(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync();

        var newWashInstruction = new WashInstruction
        {
            IsotainerTankId = request.IsotainerTankId,
            WashTypeId = request.WashTypeId,
            InstructedOn = request.InstructedOn,
        }.Create();
        
        await washContext.WashInstructions.AddAsync(newWashInstruction);
        await washContext.SaveChangesAsync();
        
        return new WashInstructionResponse(newWashInstruction.Id, newWashInstruction.IsotainerTankId, newWashInstruction.WashTypeId, newWashInstruction.InstructedOn);
    }

    public  async Task<Result<WashInstructionResponse>> UpdateWashInstruction(Guid washInstructionId, UpdateWashInstructionRequest request)
    {
        var validation = washInstructionValidator.ValidateUpdateInstruction(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        
        var washInstruction = await washContext.WashInstructions.FindAsync(washInstructionId);

        if (washInstruction == null)
        {
            return WashInstructionErrors.NotFound;
        }
        
        washInstruction.InstructedOn = request.InstructedOn;
        washInstruction.IsotainerTankId = request.IsotainerTankId;
        washInstruction.WashTypeId = request.WashTypeId;
        washInstruction.Update();
        
        washContext.WashInstructions.Update(washInstruction);
        await washContext.SaveChangesAsync();
        
        return new WashInstructionResponse(washInstruction.Id, washInstruction.IsotainerTankId, washInstruction.WashTypeId, washInstruction.InstructedOn);
    }

    public  async Task<Result<PagedList<WashInstructionItem>>> GetWashInstructions(bool isFinished, Guid? isotainerTankId, PagedRequest request)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        var query = washContext.WashInstructions.AsNoTracking();

        var totalCount = await query.CountAsync();
        
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

    public async Task<Result<List<CompletedWashInstructions>>> GetCompletedWashInstructions(Guid isotainerTankId, DateTime? from)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        
        var washInstructions = await washContext.WashInstructions
            .Where(x => x.FinishedOn != null && x.FinishedOn > from)
            .Where(x => x.IsotainerTankId == isotainerTankId)
            .Select(x => new CompletedWashInstructions(x.Id, x.IsotainerTankId, x.WashType.Type, x.WashType.Cost, x.FinishedOn.Value ))
            .ToListAsync();

        return washInstructions;
    }

    public  async Task<Result<bool>> ArchiveWashInstruction(Guid washInstructionId)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync();

        var washInstruction = await washContext.WashInstructions.FindAsync(washInstructionId);

        if (washInstruction == null)
        {
            return WashTypeErrors.NotFound;
        }

        washInstruction.Archive();

        return true;
    }
}