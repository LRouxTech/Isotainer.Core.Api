using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Isotainer.Module.Tank.Core.Cache;

public class CachedWashStatusService(IWashStatusService inner, HybridCache cache, ILogger<CachedWashStatusService> logger) : IWashStatusService
{
    public async Task<Result<PagedList<WashStatusItem>>> GetWashStatuses(PagedRequest request, CancellationToken ct = default)
    {
        string cacheKey = $"wash-statuses:page-{request.PageIndex}:size-{request.PageSize}:search-{request.Search}";

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
                cancellationToken: ct
            );

            return Result<PagedList<WashStatusItem>>.Success(pagedList);
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }
}