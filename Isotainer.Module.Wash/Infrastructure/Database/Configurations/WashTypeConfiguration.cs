using Isotainer.Module.Wash.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Wash.Infrastructure.Database.Configurations;

public static class WashTypeConfiguration
{
    public static EntityTypeBuilder<WashType> ConfigureWashType(this EntityTypeBuilder<WashType> builder)
    {
        builder.ToTable("WashType");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Cost)
            .IsRequired()
            .HasPrecision(2);

        return builder;
    }
}