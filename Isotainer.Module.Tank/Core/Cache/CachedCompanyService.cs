using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.Company;
using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using Isotainer.Module.Tank.Helpers.Cache;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Isotainer.Module.Tank.Core.Cache;

public class CachedCompanyService(ICompanyService inner, HybridCache cache, ILogger<CachedCompanyService> logger) : ICompanyService
{
    public async Task<Result<CompanyResponse>> CreateCompany(CreateCompanyRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.Company.Tag, ct);
        return await inner.CreateCompany(request, ct);
    }

    public async Task<Result<CompanyResponse>> UpdateCompany(Guid companyId, UpdateCompanyRequest request, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.Company.Tag, ct);
        return await inner.UpdateCompany(companyId, request, ct);
    }

    public async Task<Result<PagedList<CompanyItem>>> GetCompanyList(PagedRequest request, CancellationToken ct = default)
    {
        string cacheKey = CacheKeys.Company.Page(request.PageIndex, request.PageSize, request.Search);

        try
        {
            var pagedList = await cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var result = await inner.GetCompanyList(request, cancellationToken);
                
                    if (result.IsFailure)
                    {
                        logger.LogWarning($"{result.Error.Description} - {result.Error.Code}");
                        throw new ResultFailureException(result.Error);
                    }

                    return result.Value;
                },
                tags: [CacheKeys.Company.Tag],
                cancellationToken: ct
            );

            return pagedList;
        }
        catch (ResultFailureException ex)
        {
            return ex.Error;
        }
    }

    public async Task<Result<bool>> ArchiveCompany(Guid companyId, CancellationToken ct = default)
    {
        await cache.RemoveByTagAsync(CacheKeys.Company.Tag, ct);
        return await inner.ArchiveCompany(companyId, ct);
    }

    public async Task<Result<int>> GetTotalRecords(CancellationToken ct)
    {
        string cacheKey = CacheKeys.Company.TotalRecords;
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
                tags: [CacheKeys.Company.Tag],
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
        string cacheKey = CacheKeys.Company.LastUpdated;
        try
        {
            var totalRecords = await cache.GetOrCreateAsync(
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
                tags: [CacheKeys.Company.Tag],
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