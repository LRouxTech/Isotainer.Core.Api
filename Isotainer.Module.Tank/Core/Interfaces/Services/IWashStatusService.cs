using Isotainer.Module.Tank.Core.ViewModels.WashStatus;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Core.Interfaces.Services;

public interface IWashStatusService
{
    Task<Result<WashStatusListResponse>> GetWashStatuses();
}