using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using Isotainer.Module.Finance.Infrastructure.Database;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceService(IFinanceDbContextFactory dbContextFactory) : IInvoiceService
{
    public async Task<Result<CompanyInvoiceResponse>> GenerateCompanyInvoice(Guid companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IsotainerTankInvoiceResponse>> GenerateIsotainerTankInvoice(Guid tankId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<InvoiceListResponse>> GetInvoices(Guid tankId)
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var invoices = await financeContext.Invoices
            .Where(x => x.IsotainerId == tankId)
            .Select(x => new InvoiceItem(x.IsotainerId, x.InvoicedOn, x.TotalCost, x.XeroId, null))
            .ToListAsync();
        
        return new InvoiceListResponse(invoices);
    }
}