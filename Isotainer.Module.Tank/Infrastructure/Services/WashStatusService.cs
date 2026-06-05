using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class WashStatusService : IWashStatusService
{
    public Result<WashStatusListResponse> GetWashStatuses()
    {
        throw new NotImplementedException();
    }
}