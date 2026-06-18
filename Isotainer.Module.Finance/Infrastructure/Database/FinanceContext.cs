using Isotainer.Module.Finance.Core.Entities;
using Isotainer.Module.Finance.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace Isotainer.Module.Finance.Infrastructure.Database;

public class FinanceContextDesignTimeFactory : IDesignTimeDbContextFactory<FinanceContext>
{
    public FinanceContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<FinanceContext>();
        optionsBuilder.UseNpgsql(connectionString, x =>
        {
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Finance");
            x.MigrationsAssembly("Isotainer.Module.Finance");
        });

        return new FinanceContext(optionsBuilder.Options);
    }
}

public interface IFinanceDbContextFactory :  IDbContextFactory<FinanceContext>
{
}

public class FinanceDbContextFactory :  IFinanceDbContextFactory
{
    public DbContextOptions<FinanceContext> options => _options;
    private readonly DbContextOptions<FinanceContext> _options;

    public FinanceDbContextFactory(DbContextOptions<FinanceContext> options = null)
    {
        _options = options;
    }
        
    public FinanceContext CreateDbContext()
    {
        return new FinanceContext(_options);
    }
        
    public async Task<FinanceContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return new FinanceContext(_options);
    }
}


public class FinanceContext : DbContext
{
    public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
    {

    }
    
    public DbSet<GeneralCost> GeneralCosts { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        
            var connectionString = configuration.GetConnectionString("DefaultConnection");
        
            optionsBuilder.UseNpgsql(connectionString, x =>
            {
                x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Finance");
                x.MigrationsAssembly("Isotainer.Module.Finance");
            });
        }
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("Finance");

        modelBuilder.Entity<GeneralCost>().ConfigureGeneralCost();
        modelBuilder.Entity<Invoice>().ConfigureInvoice();
        modelBuilder.Entity<InvoiceLine>().ConfigureInvoiceLine();
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

            foreach (var property in properties)
            {
                property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
            }
        }
    }
}