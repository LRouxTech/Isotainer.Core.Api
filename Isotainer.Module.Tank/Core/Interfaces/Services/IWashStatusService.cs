using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using LRouxTech.Core.Auth.Infrastructure.Paged;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IWashStatusService
{
    Task<Result<PagedList<WashStatusItem>>> GetWashStatuses(PagedRequest request);
}