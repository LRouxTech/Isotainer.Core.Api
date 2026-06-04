using Isotainer.Module.Wash.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Wash.Infrastructure.Database.Configurations;

public static class WashInstructionConfiguration
{
    public static EntityTypeBuilder<WashInstruction> ConfigureWashInstruction(this EntityTypeBuilder<WashInstruction> builder)
    {
        builder.ToTable("WashInstruction");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.IsotainerTankId)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(u => u.WashTypeId)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(wi => wi.WashType)
            .WithMany(wi => wi.WashInstructions)
            .HasForeignKey(wi => wi.WashTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(u => u.InstructedOn)
            .IsRequired();

        builder.Property(u => u.FinishedOn);

        return builder;
    }
}