using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using Isotainer.Module.Finance.Infrastructure.Database;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceService(IFinanceDbContextFactory dbContextFactory, IInvoiceValidator invoiceValidator) : IInvoiceService
{
    public async Task<Result<CompanyInvoiceResponse>> GenerateCompanyInvoice(GenerateCompanyInvoiceRequest request)
    {
        var validation = invoiceValidator.ValidateGenerateCompanyInvoice(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        throw new NotImplementedException();
    }

    public async Task<Result<IsotainerTankInvoiceResponse>> GenerateIsotainerTankInvoice(GenerateIsotainerTankInvoiceRequest request)
    {
        var validation = invoiceValidator.ValidateGenerateIsotainerTankInvoice(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        throw new NotImplementedException();
    }

    public async Task<Result<InvoiceListResponse>> GetInvoices(InvoiceListRequest request)
    {
        var validation = invoiceValidator.ValidateInvoiceListRequest(request);
        if (validation.IsFailure)
        {
            return validation.Error;
        }
        
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var invoices = await financeContext.Invoices
            .Where(x => x.IsotainerId == request.IsotainerTankId)
            .Select(x => new InvoiceItem(x.IsotainerId, x.InvoicedOn, x.TotalCost, x.XeroId, null))
            .ToListAsync();
        
        return new InvoiceListResponse(invoices);
    }
}