using Isotainer.Module.Finance.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Finance.Infrastructure.Database.Configurations;

public static class InvoiceLineConfiguration
{
    public static EntityTypeBuilder<InvoiceLine> ConfigureInvoiceLine(this EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.ToTable("InvoiceLine");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.InvoiceId)
            .IsRequired();

        builder.HasOne(il => il.Invoice)
            .WithMany(il => il.InvoiceLines)
            .HasForeignKey(il => il.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(u => u.ItemName)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(u => u.Cost)
            .IsRequired();
        
        return builder;
    }
}