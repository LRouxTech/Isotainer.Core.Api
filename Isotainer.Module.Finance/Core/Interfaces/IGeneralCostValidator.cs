using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Core.Interfaces;

public interface IGeneralCostValidator
{
    Result<bool> ValidateUpdateGeneralCost(UpdateGeneralCostRequest request);
}