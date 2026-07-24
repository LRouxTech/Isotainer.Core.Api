using Isotainer.Core.Api.tempmodels;
using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using Isotainer.Module.Tank.Infrastructure.Database;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class WashStatusService(ITankDbContextFactory dbContextFactory) : IWashStatusService
{
    public async Task<Result<PagedList<WashStatusItem>>> GetWashStatuses(PagedRequest request)
    {
        await using var tankContext = await dbContextFactory.CreateDbContextAsync();
        var query = tankContext.WashStatus.AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Id)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new WashStatusItem(x.Id, x.Type.ToString()))
            .ToListAsync();

        return new PagedList<WashStatusItem>(items, totalCount, request.PageIndex, request.PageSize);
    }
}