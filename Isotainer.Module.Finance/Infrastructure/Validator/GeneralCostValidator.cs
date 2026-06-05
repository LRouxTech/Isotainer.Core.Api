using Isotainer.Module.Finance.Core.Interfaces;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.GeneralCost;
using Isotainer.Module.Finance.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Finance.Infrastructure.Validator;

public class GeneralCostValidator : IGeneralCostValidator
{
    public Result<bool> ValidateUpdateGeneralCost(UpdateGeneralCostRequest request)
    {
        if (request.Cost < 0)
        {
            return GeneralCostErrors.InvalidCost;
        }
        return true;
    }
}