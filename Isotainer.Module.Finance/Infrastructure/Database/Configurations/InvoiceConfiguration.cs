using Isotainer.Module.Finance.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Finance.Infrastructure.Database.Configurations;

public static class InvoiceConfiguration
{
    public static EntityTypeBuilder<Invoice> ConfigureInvoice(this EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoice");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.IsotainerId)
            .IsRequired();

        builder.Property(u => u.InvoicedOn)
            .IsRequired();
        
        builder.Property(u => u.TotalCost)
            .IsRequired();
        
        builder.Property(u => u.XeroId)
            .HasMaxLength(300);
        
        return builder;
    }
}