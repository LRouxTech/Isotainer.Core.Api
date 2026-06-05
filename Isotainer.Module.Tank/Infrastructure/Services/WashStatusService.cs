using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using Isotainer.Module.Tank.Infrastructure.Database;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class WashStatusService(ITankDbContextFactory dbContextFactory) : IWashStatusService
{
    public async Task<Result<WashStatusListResponse>> GetWashStatuses()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var washStatuses = dbContext.WashStatus
            .Select(x => new WashStatusItem(x.Id, x.Type.ToString()))
            .ToList();
        
        return new WashStatusListResponse(washStatuses);
    }
}