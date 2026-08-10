using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using Isotainer.Module.Tank.Helpers.Cache;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Isotainer.Module.Tank.Core.Cache;

public class CachedWashStatusService(IWashStatusService inner, HybridCache cache, ILogger<CachedWashStatusService> logger) : IWashStatusService
{
    public async Task<Result<PagedList<WashStatusItem>>> GetWashStatuses(PagedRequest request, CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.WashStatus.Page(request.PageIndex, request.PageSize, request.Search);

        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetWashStatuses(request, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.WashStatus.Tag],
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