using Isotainer.Module.Tank.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Tank.Infrastructure.Database.Configurations;

public static class WashStatusConfiguration
{
    public static EntityTypeBuilder<WashStatus> ConfigureWashStatus(this EntityTypeBuilder<WashStatus> builder)
    {
        builder.ToTable("WashStatus");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.Type)
            .IsRequired()
            .HasMaxLength(200);

        return builder;
    }
}