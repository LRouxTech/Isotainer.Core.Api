using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Finance.Core.Entities;

public class GeneralCosts : BaseModel<GeneralCosts>
{
    public GeneralCostEnum CostItem { get; set; }
    public double Cost { get; set; }
}

public enum GeneralCostEnum {
    Storage = 1,
}