using Isotainer.Module.Tank.Core.Cache;
using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using Isotainer.Module.Wash.Helpers.Cache;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Isotainer.Module.Wash.Core.Cache;

public class CachedWashTypeService(IWashTypeService inner, HybridCache cache, ILogger<CachedWashTypeService> logger) : IWashTypeService
{
    public async Task<Result<WashTypeResponse>> CreateWashType(CreateWashTypeRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.WashType.Tag, ct);
        return await inner.CreateWashType(request, ct);
    }

    public async Task<Result<WashTypeResponse>> UpdateWashType(Guid washTypeId, UpdateWashTypeRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.WashType.Tag, ct);
        return await inner.UpdateWashType(washTypeId, request, ct);
    }

    public async Task<Result<PagedList<WashTypeItem>>> GetWashTypes(PagedRequest request, CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.WashType.Page(request.PageIndex, request.PageSize, request.Search);

        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetWashTypes(request, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashType.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<bool>> ArchiveWashType(Guid washTypeId, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.WashType.Tag, ct);
        return await inner.ArchiveWashType(washTypeId, ct);
    }

    public async Task<Result<int>> GetTotalRecords(CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.WashType.TotalRecords;
        try
        {
            var totalRecords = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetTotalRecords(cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashType.Tag],
                cancellationToken: ct
            );

            return totalRecords;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<DateTime>> GetLastUpdated(CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.WashType.LastUpdated;
        try
        {
            var lastUpdated = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetLastUpdated(cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashType.Tag],
                cancellationToken: ct
            );

            return lastUpdated;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }
}