using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Tanks.Core.Entities;

public class WashStatus : BaseModel<WashStatus>
{
    public required string Type { get; set; }
}