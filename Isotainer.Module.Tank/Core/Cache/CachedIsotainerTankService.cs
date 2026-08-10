using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using Isotainer.Module.Tank.Helpers.Cache;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Isotainer.Module.Tank.Core.Cache;

public class CachedIsotainerTankService(IIsotainerTankService inner, HybridCache cache, ILogger<CachedIsotainerTankService> logger) : IIsotainerTankService
{
    public async Task<Result<IsotainerTankResponse>> CreateIsotainerTank(CreateIsotainerTankRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.IsotainerTank.Tag, ct);
        return await inner.CreateIsotainerTank(request, ct);
    }

    public async Task<Result<IsotainerTankResponse>> UpdateIsotainerTank(Guid isotainerTankId, UpdateIsotainerTankRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.IsotainerTank.Tag, ct);
        return await inner.UpdateIsotainerTank(isotainerTankId, request, ct);
    }

    public async Task<Result<PagedList<IsotainerTankItem>>> GetIsotainerTanks(PagedRequest request, CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.IsotainerTank.Page(request.PageIndex, request.PageSize, request.Search);
        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetIsotainerTanks(request, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.IsotainerTank.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<Dictionary<Guid, string>>> GetIsotainerTanks(List<Guid> ids, CancellationToken ct = default)
    {
        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                CacheKeys.IsotainerTank.ByIds,
                async cancellationToken =>
                {
                    var result = await inner.GetIsotainerTanks(ids, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.IsotainerTank.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<IsotainerTank>> GetIsotainerTankDetails(Guid id, CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.IsotainerTank.ById(id);
        try
        {
            var totalRecords = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetIsotainerTankDetails(id, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.IsotainerTank.Tag],
                cancellationToken: ct
            );

            return totalRecords;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<bool>> ArchiveIsotainerTank(Guid isotainerTankId, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.IsotainerTank.Tag, ct);
        return await inner.ArchiveIsotainerTank(isotainerTankId, ct);
    }

    public async Task<Result<IsotainerTankResponse>> ChangeWashStatus(Guid isotainerTankId, ChangeWashStatusRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.IsotainerTank.Tag, ct);
        return await inner.ChangeWashStatus(isotainerTankId, request, ct);
    }

    public async Task<Result<IsotainerTankResponse>> UnloadTank(Guid isotainerTankId, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.IsotainerTank.Tag, ct);
        return await inner.UnloadTank(isotainerTankId, ct);
    }

    public async Task<Result<int>> GetTotalActiveTanks(CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.IsotainerTank.TotalActiveTanks;
        try
        {
            var totalRecords = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetTotalActiveTanks(cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.IsotainerTank.Tag],
                cancellationToken: ct
            );

            return totalRecords;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<int>> GetNewInventory(CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.IsotainerTank.NewInventory;
        try
        {
            var totalRecords = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetNewInventory(cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.IsotainerTank.Tag],
                cancellationToken: ct
            );

            return totalRecords;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<string>> GetAverageTurnaroundTime(CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.IsotainerTank.AverageTurnaroundTime;
        try
        {
            var totalRecords = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetAverageTurnaroundTime(cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.IsotainerTank.Tag],
                cancellationToken: ct
            );

            return totalRecords;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }
}