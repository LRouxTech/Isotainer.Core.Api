using Isotainer.Core.Api.Auth;
using Isotainer.Module.Finance.Infrastructure.Database;
using Isotainer.Module.Finance.Infrastructure.Database.Seed;
using Isotainer.Module.Tank.Infrastructure.Database;
using Isotainer.Module.Tank.Infrastructure.Database.Seed;
using Isotainer.Module.Wash.Infrastructure.Database;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Api.Extensions;
using LRouxTech.Core.Auth.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddScoped<IUserDbContextFactory, UserDbContextFactory>();

if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    var conString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    builder.Services.AddDbContextFactory<UserContext, UserDbContextFactory>(options =>
    {
        options.UseNpgsql(conString, x =>
        {
            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "User");
            x.MigrationsAssembly("LRouxTech.Core.Auth");
        });
    });
    
    builder.Services.AddDbContextFactory<FinanceContext, FinanceDbContextFactory>(options =>
    {
        options.UseNpgsql(conString, x =>
        {
            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Finance");
            x.MigrationsAssembly("Isotainer.Module.Finance");
        });
    });
    
    builder.Services.AddDbContextFactory<TankContext, TankDbContextFactory>(options =>
    {
        options.UseNpgsql(conString, x =>
        {
            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Tank");
            x.MigrationsAssembly("Isotainer.Module.Tank");
        });
    });
    
    builder.Services.AddDbContextFactory<WashContext, WashDbContextFactory>(options =>
    {
        options.UseNpgsql(conString, x =>
        {
            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Wash");
            x.MigrationsAssembly("Isotainer.Module.Wash");
        });
    });
}

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddAuthModule();
builder.Services.AddCustomPermissions<IsotainerPermissions>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var contextFactory = services.GetRequiredService<IDbContextFactory<UserContext>>();
        await using var context = await contextFactory.CreateDbContextAsync();
            
        await context.Database.MigrateAsync(); 
        
        await RuntimeDataSeeder.SeedPermissionsAsync<IsotainerPermissions>(context);
        
        await RuntimeDataSeeder.SeedRolesAsync<IsotainerRoles>(context);

        await RuntimeDataSeeder.SeedAdminUserAsync(context);

        await RuntimeDataSeeder.SyncRolePermissionsAsync(context, IsotainerRoles.TankAdmin, [
            IsotainerPermissions.Tank.ViewCompanies,
            IsotainerPermissions.Tank.ViewIsotainers,
            IsotainerPermissions.Tank.CreateIsotainer,
            IsotainerPermissions.Tank.UpdateIsotainer,
            IsotainerPermissions.Tank.ChangeIsotainerWashStatus,
            IsotainerPermissions.Tank.UnloadIsotainer,
            IsotainerPermissions.Tank.ViewWashStatuses,
            IsotainerPermissions.Wash.ViewWashTypes,
            IsotainerPermissions.Wash.ViewWashInstructions,
            IsotainerPermissions.Wash.CreateWashInstruction,
            IsotainerPermissions.Wash.UpdateWashInstruction,
            IsotainerPermissions.Wash.DeleteWashInstruction,
        ]);
        
        await RuntimeDataSeeder.SyncRolePermissionsAsync(context, IsotainerRoles.FinanceAdmin, [
            IsotainerPermissions.Tank.ViewCompanies,
            IsotainerPermissions.Tank.ViewIsotainers,
            IsotainerPermissions.Tank.ViewWashStatuses,
            IsotainerPermissions.Finance.ViewGeneralCosts,
            IsotainerPermissions.Finance.ViewTankInvoices,
            IsotainerPermissions.Finance.ViewCompanyInvoices,
            IsotainerPermissions.Finance.GenerateTankInvoice,
            IsotainerPermissions.Finance.ViewInvoiceLines,
        ]);
        
        await RuntimeDataSeeder.SyncRolePermissionsAsync(context, IsotainerRoles.Washer, [
            IsotainerPermissions.Tank.ViewCompanies,
            IsotainerPermissions.Tank.ViewIsotainers,
            IsotainerPermissions.Tank.ViewWashStatuses,
            IsotainerPermissions.Wash.ViewWashTypes,
            IsotainerPermissions.Wash.ViewWashInstructions,
            IsotainerPermissions.Wash.UpdateWashInstruction,
        ]);
        
        var financeFactory = services.GetRequiredService<IDbContextFactory<FinanceContext>>();
        await using var financeContext = await financeFactory.CreateDbContextAsync();
            
        await financeContext.Database.MigrateAsync();

        await FinanceDataSeeder.SeedGeneralCostsAsync(financeContext);
        
        var tankFactory = services.GetRequiredService<IDbContextFactory<TankContext>>();
        await using var tankContext = await tankFactory.CreateDbContextAsync();
            
        await TankDataSeeder.SeedWashStatusAsync(tankContext);
        await tankContext.Database.MigrateAsync(); 
        
        var washFactory = services.GetRequiredService<IDbContextFactory<WashContext>>();
        await using var washContext = await washFactory.CreateDbContextAsync();
            
        await washContext.Database.MigrateAsync(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization or data seeding.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();