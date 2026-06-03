using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Wash.Core.Entities;

public class WashType : BaseModel<WashType>
{
    public required string Type { get; set; }
    public double Cost { get; set; }
}