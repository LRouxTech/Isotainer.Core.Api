using Isotainer.Module.Tank.Core.Entities;
using LRouxTech.Core.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isotainer.Module.Tank.Infrastructure.Database.Configurations;

public static class CompanyConfiguration
{
    public static EntityTypeBuilder<Company> ConfigureCompany(this EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Company");
        builder.ConfigureBaseModel();
        
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        return builder;
    }
}