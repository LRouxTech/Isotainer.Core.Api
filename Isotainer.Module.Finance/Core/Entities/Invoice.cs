using LRouxTech.Core.BaseModel;

namespace Isotainer.Module.Finance.Core.Entities;

public class Invoice : BaseModel<Invoice>
{
    public Guid IsotainerId { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime InvoicedOn { get; set; }
    public double TotalCost { get; set; }
    public string? XeroId { get; set; }
    
    public virtual ICollection<InvoiceLine>? InvoiceLines { get; set; }
}