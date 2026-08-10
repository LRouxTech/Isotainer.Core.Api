using Isotainer.Module.Tank.Core.Cache;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashInstruction;
using Isotainer.Module.Wash.Helpers.Cache;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Isotainer.Module.Wash.Core.Cache;

public class CachedWashInstructionService(IWashInstructionService inner, HybridCache cache, ILogger<CachedWashInstructionService> logger) : IWashInstructionService
{
    public async Task<Result<WashInstructionResponse>> CreateWashInstruction(CreateWashInstructionRequest request, CancellationToken ct)
    {
        await cache.RemoveByTagAsync(CacheKeys.WashInstruction.Tag, ct);
        return await inner.CreateWashInstruction(request, ct);
    }

    public async Task<Result<WashInstructionResponse>> UpdateWashInstruction(Guid washInstructionId, UpdateWashInstructionRequest request, CancellationToken ct)
    {
        await cache.RemoveByTagAsync(CacheKeys.WashInstruction.Tag, ct);
        return await inner.UpdateWashInstruction(washInstructionId, request, ct);
    }

    public async Task<Result<PagedList<WashInstructionItem>>> GetWashInstructions(bool isFinished, Guid? isotainerTankId, PagedRequest request, CancellationToken ct)
    {
        string cacheKey = CacheKeys.WashInstruction.Page(request.PageIndex, request.PageSize, request.Search);

        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetWashInstructions(isFinished, isotainerTankId, request, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashInstruction.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<List<CompletedWashInstructions>>> GetCompletedWashInstructions(Guid isotainerTankId, DateTime? from, CancellationToken ct)
    {
        string cacheKey = CacheKeys.WashInstruction.CompletedInstructions(isotainerTankId, from);

        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetCompletedWashInstructions(isotainerTankId, from, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashInstruction.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<bool>> ArchiveWashInstruction(Guid washInstructionId, CancellationToken ct)
    {
        await cache.RemoveByTagAsync(CacheKeys.WashInstruction.Tag, ct);
        return await inner.ArchiveWashInstruction(washInstructionId, ct);
    }

    public async Task<Result<int>> GetTotalWashesBooked(CancellationToken ct)
    {
        string cacheKey = CacheKeys.WashInstruction.TotalWashesBooked;

        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetTotalWashesBooked(cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashInstruction.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }
}