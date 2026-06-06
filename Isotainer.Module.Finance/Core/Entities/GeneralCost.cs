using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Finance.Core.Entities;

public class GeneralCost : BaseModel<GeneralCost>
{
    public GeneralCostEnum CostItem { get; set; }
    public double Cost { get; set; }
}

public enum GeneralCostEnum {
    Storage = 1,
    Liftin = 2,
    Liftout = 3,
}