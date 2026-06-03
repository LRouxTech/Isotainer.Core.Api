using Isotainer.Module.Tank.Core.Entities;
using Isotainer.Module.Tank.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace Isotainer.Module.Tank.Infrastructure.Database;

public class TankContextDesignTimeFactory : IDesignTimeDbContextFactory<TankContext>
{
    public TankContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<TankContext>();
        optionsBuilder.UseNpgsql(connectionString, x =>
        {
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Tank");
            x.MigrationsAssembly("LRouxTech.Core.Auth");
        });

        return new TankContext(optionsBuilder.Options);
    }
}

public interface ITankDbContextFactory :  IDbContextFactory<TankContext>
{
}

public class TankDbContextFactory :  ITankDbContextFactory
{
    public DbContextOptions<TankContext> options => _options;
    private readonly DbContextOptions<TankContext> _options;

    public TankDbContextFactory(DbContextOptions<TankContext> options = null)
    {
        _options = options;
    }
        
    public TankContext CreateDbContext()
    {
        return new TankContext(_options);
    }
        
    public async Task<TankContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return new TankContext(_options);
    }
}


public class TankContext : DbContext
{
    public TankContext(DbContextOptions<TankContext> options) : base(options)
    {

    }
    
    public DbSet<Company> Companies { get; set; }
    public DbSet<IsotainerTank> IsotainerTanks { get; set; }
    public DbSet<WashStatus> WashStatus { get; set; }
    
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
                x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Tank");
                x.MigrationsAssembly("Isotainer.Module.Tank");
            });
        }
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().ConfigureCompany();
        modelBuilder.Entity<WashStatus>().ConfigureWashStatus();
        modelBuilder.Entity<IsotainerTank>().ConfigureIsotainerTank();
        
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