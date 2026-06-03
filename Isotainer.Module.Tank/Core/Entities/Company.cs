using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Tank.Core.Entities;

public class Company : BaseModel<Company>
{
    public required string Name { get; set; }
    
    public virtual ICollection<IsotainerTank>? IsotainerTanks { get; set; }

}