using Isotainer.Module.Wash.Core.Entities;
using Isotainer.Module.Wash.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace Isotainer.Module.Wash.Infrastructure.Database;


public class WashContextDesignTimeFactory : IDesignTimeDbContextFactory<WashContext>
{
    public WashContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<WashContext>();
        optionsBuilder.UseNpgsql(connectionString, x =>
        {
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Wash");
            x.MigrationsAssembly("Isotainer.Module.Wash");
        });

        return new WashContext(optionsBuilder.Options);
    }
}

public interface IWashDbContextFactory :  IDbContextFactory<WashContext>
{
}

public class WashDbContextFactory :  IWashDbContextFactory
{
    public DbContextOptions<WashContext> options => _options;
    private readonly DbContextOptions<WashContext> _options;

    public WashDbContextFactory(DbContextOptions<WashContext> options = null)
    {
        _options = options;
    }
        
    public WashContext CreateDbContext()
    {
        return new WashContext(_options);
    }
        
    public async Task<WashContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return new WashContext(_options);
    }
}


public class WashContext : DbContext
{
    public WashContext(DbContextOptions<WashContext> options) : base(options)
    {

    }
    
    public DbSet<WashInstruction> WashInstructions { get; set; }
    public DbSet<WashType> WashTypes { get; set; }
    
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
                x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Wash");
                x.MigrationsAssembly("Isotainer.Module.Wash");
            });
        }
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Wash");

        modelBuilder.Entity<WashInstruction>().ConfigureWashInstruction();
        modelBuilder.Entity<WashType>().ConfigureWashType();
        
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