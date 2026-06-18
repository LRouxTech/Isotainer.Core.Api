using Isotainer.Module.Tank.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Isotainer.Module.Tank.Infrastructure.Database.Seed;

public static class TankDataSeeder
{
    public static async Task SeedWashStatusAsync(TankContext context)
    {
        var washEnumValues = Enum.GetValues<WashStatusEnum>();

        foreach (var washEnum in washEnumValues)
        {
            var exists = await context.WashStatus.AnyAsync(gc => gc.Type == washEnum);
            if (!exists)
            {
                var washStatus = new WashStatus
                {
                    Id = Guid.CreateVersion7(),
                    Type = washEnum
                };

                await context.WashStatus.AddAsync(washStatus);
            }
        }

        await context.SaveChangesAsync();
    }
}