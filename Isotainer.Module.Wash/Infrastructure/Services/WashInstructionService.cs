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

    public  async Task<Result<WashInstructionResponse>> UpdateWashInstruction(UpdateWashInstructionRequest request)
    {
        var validation = washInstructionValidator.ValidateUpdateInstruction(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        
        var washInstruction = await washContext.WashInstructions.FindAsync(request.WashInstructionId);

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

    public  async Task<Result<WashInstructionsListResponse>> GetWashInstructions(WashInstructionListRequest request)
    {
        await using var washContext = await dbContextFactory.CreateDbContextAsync();
        var washTypes = await washContext.WashTypes.ToListAsync();
        
        var washInstructions = await washContext.WashInstructions
            .Select(x => new { x.Id, x.IsotainerTankId, x.InstructedOn, x.WashTypeId })
            .ToListAsync();

        return new WashInstructionsListResponse([]);
    }

    public  async Task<Result<bool>> ArchiveWashInstruction(ArchiveWashInstructionRequest request)
    {
        var validation = washInstructionValidator.ValidateArchiveInstruction(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var washContext = await dbContextFactory.CreateDbContextAsync();

        var washInstruction = await washContext.WashInstructions.FindAsync(request.WashInstructionId);

        if (washInstruction == null)
        {
            return WashTypeErrors.NotFound;
        }

        washInstruction.Archive();

        return true;
    }
}