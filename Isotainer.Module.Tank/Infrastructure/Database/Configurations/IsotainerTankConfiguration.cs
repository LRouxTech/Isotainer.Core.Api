using Isotainer.Module.Tank.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Tank.Infrastructure.Database.Configurations;

public static class IsotainerTankConfiguration
{
    public static EntityTypeBuilder<IsotainerTank> ConfigureIsotainerTank(this EntityTypeBuilder<IsotainerTank> builder)
    {
        builder.ToTable("IsotainerTank");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.TankNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.WashStatusId)
            .IsRequired();

        builder.HasOne(it => it.WashStatus)
            .WithMany(it => it.IsotainerTanks)
            .HasForeignKey(it => it.WashStatusId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(u => u.CompanyId)
            .IsRequired();

        builder.HasOne(it => it.Company)
            .WithMany(it => it.IsotainerTanks)
            .HasForeignKey(it => it.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(u => u.LoadedOn)
            .IsRequired();
        
        builder.Property(u => u.UnloadedOn);

        return builder;
    }
}