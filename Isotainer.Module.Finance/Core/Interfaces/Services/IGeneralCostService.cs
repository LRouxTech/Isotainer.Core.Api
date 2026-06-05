using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces.Services;

public interface IGeneralCostService
{
    Task<Result<GeneralCostUpdateResponse>> UpdateGeneralCost(Guid Id, UpdateGeneralCostRequest request);
    Task<Result<GeneralCostListResponse>> GetGeneralCosts();
}