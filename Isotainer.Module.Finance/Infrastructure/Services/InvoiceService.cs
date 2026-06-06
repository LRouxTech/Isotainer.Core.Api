using Isotainer.Module.Finance.Core.Entities;
using Isotainer.Module.Finance.Core.Interfaces.Services;
using Isotainer.Module.Finance.Core.Interfaces.Validators;
using Isotainer.Module.Finance.Core.ViewModels.Invoice;
using Isotainer.Module.Finance.Infrastructure.Database;
using Isotainer.Module.Finance.Infrastructure.Errors;
using LRouxTech.Core.ValidationResult;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Services;

public class InvoiceService(IFinanceDbContextFactory dbContextFactory) : IInvoiceService
{
    public async Task<Result<IsotainerTankInvoiceResponse>> GenerateIsotainerTankInvoice(GenerateIsotainerInvoiceRequest request)
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var newInvoice = new Invoice
        {
            IsotainerId = request.tankId,
            CompanyId = request.companyId,
            InvoicedOn = DateTime.UtcNow,
            TotalCost = 0,
            InvoiceLines = []
        }.Create();
        
        #region Storage
        var storage = 0;
        
        if (request.LastInvoiceDate == null)
        {
            storage = (DateTime.UtcNow - request.LoadedOn).Days;
        }
        else
        {
            storage = (DateTime.UtcNow - request.LastInvoiceDate.Value).Days;
        }

        var storageCost = await financeContext.GeneralCosts.FirstOrDefaultAsync(x => x.CostItem == GeneralCostEnum.Storage);
        if (storageCost == null)
        {
            return GeneralCostErrors.NotFound;
        }

        newInvoice.InvoiceLines?.Add(new InvoiceLine
        {
            ItemName = "Storage",
            Cost = storage * storageCost.Cost,
        }.Create());
        
        #endregion

        #region Lift In

        if (request.LastInvoiceDate == null)
        {
            var liftInCost = await financeContext.GeneralCosts.FirstOrDefaultAsync(x => x.CostItem == GeneralCostEnum.Liftin);
            if (liftInCost == null)
            {
                return GeneralCostErrors.NotFound;
            }
            newInvoice.InvoiceLines?.Add(new InvoiceLine
            {
                ItemName = "Lift In",
                Cost = liftInCost.Cost,
            }.Create());
        }

        #endregion

        #region Lift Out

        if (request.UnloadedOn != null && request.LastInvoiceDate < request.UnloadedOn)
        {
            var liftOutCost = await financeContext.GeneralCosts.FirstOrDefaultAsync(x => x.CostItem == GeneralCostEnum.Liftout);
            if (liftOutCost == null)
            {
                return GeneralCostErrors.NotFound;
            }
            newInvoice.InvoiceLines?.Add(new InvoiceLine
            {
                ItemName = "Lift Out",
                Cost = liftOutCost.Cost,
            }.Create());
        }

        #endregion

        #region Washes

        foreach (var wash in request.washItems)
        {
            newInvoice.InvoiceLines?.Add(new InvoiceLine
            {
                ItemName = $"{wash.wash} on {wash.date:dd/MM/yyyy}",
                Cost = wash.cost,
            }.Create());
        }

        if (newInvoice.InvoiceLines?.ToList() is null or [])
        {
            return InvoiceErrors.NoLines;
        }

        newInvoice.TotalCost = newInvoice.InvoiceLines.Sum(x => x.Cost);
        
        await financeContext.Invoices.AddAsync(newInvoice);
        await financeContext.SaveChangesAsync();

        #endregion
        
        return new IsotainerTankInvoiceResponse(newInvoice.IsotainerId, newInvoice.CompanyId, newInvoice.InvoicedOn, newInvoice.TotalCost, newInvoice.InvoiceLines.Select(x => new WashItems(x.ItemName, x.Cost, newInvoice.InvoicedOn)).ToList() );
    }

    public async Task<Result<InvoiceListResponse>> GetInvoices(Guid tankId)
    {
        await using var financeContext = await dbContextFactory.CreateDbContextAsync();
        var invoices = await financeContext.Invoices
            .Where(x => x.IsotainerId == tankId)
            .Select(x => new InvoiceItem(x.IsotainerId, x.CompanyId, x.InvoicedOn, x.TotalCost, x.XeroId, null))
            .ToListAsync();
        
        return new InvoiceListResponse(invoices);
    }
}