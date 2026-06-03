using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Tank.Core.Entities;

public class WashStatus : BaseModel<WashStatus>
{
    public required WashStatusEnum Type { get; set; }
    
    public virtual ICollection<IsotainerTank>? IsotainerTanks { get; set; }
}

public enum WashStatusEnum
{
    New = 1,
    Booked = 2,
    Clean = 3,
}