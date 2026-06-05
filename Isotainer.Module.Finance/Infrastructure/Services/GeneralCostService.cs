using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class GeneralCostService : IGeneralCostService
{
    public Result<GeneralCostUpdateResponse> UpdateGeneralCost(UpdateGeneralCostRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<GeneralCostListResponse> GetGeneralCosts()
    {
        throw new NotImplementedException();
    }
}