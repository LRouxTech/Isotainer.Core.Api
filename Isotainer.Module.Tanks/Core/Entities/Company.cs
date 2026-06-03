using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Tanks.Core.Entities;

public class Company : BaseModel<Company>
{
    public required string Name { get; set; }
}