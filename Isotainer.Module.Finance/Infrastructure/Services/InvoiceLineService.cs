using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.InvoiceLine;
using Isotainer.Module.Finance.Infrastructure.Database;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceLineService(IFinanceDbContextFactory dbContextFactory, IInvoiceLineValidator invoiceLineValidator) : IInvoiceLineService
{
    public async Task<Result<InvoiceLineListResponse>> GetInvoiceLines(InvoiceLineListRequest request)
    {
        var validation = invoiceLineValidator.ValidateInvoiceLineListRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var invoiceLines = await financeContext.InvoiceLines
            .Where(x => x.InvoiceId == request.InvoiceId)
            .Select(x => new InvoiceLineItem(x.Id, x.ItemName, x.Cost))
            .ToListAsync();
        
        return new InvoiceLineListResponse(invoiceLines);
    }
}