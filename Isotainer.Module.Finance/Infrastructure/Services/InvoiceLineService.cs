using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using Isotainer.Module.Finance.Infrastructure.Database;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceLineService(IFinanceDbContextFactory dbContextFactory) : IInvoiceLineService
{
    public async Task<Result<InvoiceLineListResponse>> GetInvoiceLines(Guid invoiceId)
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var invoiceLines = await financeContext.InvoiceLines
            .Where(x => x.InvoiceId == invoiceId)
            .Select(x => new InvoiceLineItem(x.Id, x.ItemName, x.Cost))
            .ToListAsync();
        
        return new InvoiceLineListResponse(invoiceLines);
    }
}