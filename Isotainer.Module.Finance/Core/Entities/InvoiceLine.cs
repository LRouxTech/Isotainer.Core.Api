using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Finance.Core.Entities;

public class InvoiceLine : BaseModel<InvoiceLine>
{
    public Guid InvoiceId { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public required string ItemName { get; set; }
    public double Cost { get; set; }
}