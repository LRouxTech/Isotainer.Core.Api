using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Tanks.Core.Entities;

public class Isotainer : BaseModel<Isotainer>
{
    public required string TankNumber { get; set; }
    public Guid WashStatusId { get; set; }
    public virtual WashStatus? WashStatus { get; set; }
    public Guid CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    public DateTime LoadedOn { get; set; }
    public DateTime? UnloadedOn { get; set; }
}