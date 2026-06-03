using Isotainer.Module.Finance.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Finance.Infrastructure.Database.Configurations;

public static class GeneralCostConfiguration
{
    public static EntityTypeBuilder<GeneralCost> ConfigureGeneralCost(this EntityTypeBuilder<GeneralCost> builder)
    {
        builder.ToTable("GeneralCost");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.CostItem)
            .IsRequired();

        builder.Property(u => u.Cost)
            .IsRequired();
        return builder;
    }
}