using Isotainer.Module.Finance.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Finance.Infrastructure.Database.Seed;

public static class FinanceDataSeeder
{
    public static async Task SeedGeneralCostsAsync(FinanceContext context)
    {
        var costEnumValues = Enum.GetValues<GeneralCostEnum>();

        foreach (var costEnum in costEnumValues)
        {
            var exists = await context.GeneralCosts.AnyAsync(gc => gc.CostItem == costEnum);
            if (!exists)
            {
                var generalCost = new GeneralCost
                {
                    Id = Guid.CreateVersion7(),
                    CostItem = costEnum,
                    Cost = 0
                };

                await context.GeneralCosts.AddAsync(generalCost);
            }
        }

        await context.SaveChangesAsync();
    }
}