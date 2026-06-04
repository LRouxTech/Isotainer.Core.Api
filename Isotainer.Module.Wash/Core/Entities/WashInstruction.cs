using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Wash.Core.Entities;

public class WashInstruction : BaseModel<WashInstruction>
{
    public Guid IsotainerTankId { get; set; }
    public Guid WashTypeId { get; set; }
    public virtual WashType? WashType { get; set; }
    public DateTime InstructedOn { get; set; }
    public DateTime? FinishedOn { get; set; }
}