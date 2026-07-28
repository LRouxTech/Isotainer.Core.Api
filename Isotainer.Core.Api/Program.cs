using Isotainer.Core.Api.Auth;
using Isotainer.Core.Api.Endpoints.Extensions;
using Isotainer.Core.Api.Endpoints.Tank;
using Isotainer.Core.Api.Endpoints.Wash;
using Isotainer.Module.Tank.Infrastructure.Database;
using Isotainer.Module.Tank.Infrastructure.Database.Seed;
using Isotainer.Module.Wash.Infrastructure.Database;
using LRouxTech.Core.Auth.Api.Authorization;
using LRouxTech.Core.Auth.Api.Endpoints;
using LRouxTech.Core.Auth.Api.Extensions;
using LRouxTech.Core.Auth.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() 
                     ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

builder.Services.AddTankModule();
builder.Services.AddWashModule();

var app = builder.Build();

app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userFactory = services.GetRequiredService<IDbContextFactory<UserContext>>();
        var tankFactory = services.GetRequiredService<IDbContextFactory<TankContext>>();
        var washFactory = services.GetRequiredService<IDbContextFactory<WashContext>>();

        await using var userContext = await userFactory.CreateDbContextAsync();
        await using var tankContext = await tankFactory.CreateDbContextAsync();
        await using var washContext = await washFactory.CreateDbContextAsync();

        await userContext.Database.MigrateAsync(); 
        await tankContext.Database.MigrateAsync(); 
        await washContext.Database.MigrateAsync(); 

        await RuntimeDataSeeder.SeedPermissionsAsync<IsotainerPermissions>(userContext);
        await RuntimeDataSeeder.SeedRolesAsync<IsotainerRoles>(userContext);
        await RuntimeDataSeeder.SeedAdminUserAsync(userContext);
        
        await RuntimeDataSeeder.SyncRolePermissionsAsync(userContext, IsotainerRoles.Admin, [
            IsotainerPermissions.UserManagement.Create,
            IsotainerPermissions.UserManagement.Read,
            IsotainerPermissions.UserManagement.Update,
            IsotainerPermissions.UserManagement.Delete,
            
            IsotainerPermissions.Tank.ViewCompanies,
            IsotainerPermissions.Tank.CreateCompany,
            IsotainerPermissions.Tank.UpdateCompany,
            IsotainerPermissions.Tank.DeleteCompany,
            
            IsotainerPermissions.Tank.ViewIsotainers,
            IsotainerPermissions.Tank.CreateIsotainer,
            IsotainerPermissions.Tank.UpdateIsotainer,
            IsotainerPermissions.Tank.ChangeIsotainerWashStatus,
            IsotainerPermissions.Tank.UnloadIsotainer,
            IsotainerPermissions.Tank.DeleteIsotainer,
            
            IsotainerPermissions.Tank.ViewWashStatuses,
            
            IsotainerPermissions.Finance.ViewGeneralCosts,
            IsotainerPermissions.Finance.UpdateGeneralCosts,
            IsotainerPermissions.Finance.ViewTankInvoices,
            IsotainerPermissions.Finance.ViewCompanyInvoices,
            IsotainerPermissions.Finance.ViewInvoice,
            IsotainerPermissions.Finance.GenerateTankInvoice,
            IsotainerPermissions.Finance.ViewInvoiceLines,
            
            IsotainerPermissions.Wash.ViewWashTypes,
            IsotainerPermissions.Wash.CreateWashType,
            IsotainerPermissions.Wash.UpdateWashType,
            IsotainerPermissions.Wash.DeleteWashType,
            IsotainerPermissions.Wash.ViewWashInstructions,
            IsotainerPermissions.Wash.CreateWashInstruction,
            IsotainerPermissions.Wash.UpdateWashInstruction,
            IsotainerPermissions.Wash.DeleteWashInstruction,

        ]);

        await RuntimeDataSeeder.SyncRolePermissionsAsync(userContext, IsotainerRoles.TankAdmin, [
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
        
        await RuntimeDataSeeder.SyncRolePermissionsAsync(userContext, IsotainerRoles.FinanceAdmin, [
            IsotainerPermissions.Tank.ViewCompanies,
            IsotainerPermissions.Tank.ViewIsotainers,
            IsotainerPermissions.Tank.ViewWashStatuses,
            IsotainerPermissions.Finance.ViewGeneralCosts,
            IsotainerPermissions.Finance.ViewTankInvoices,
            IsotainerPermissions.Finance.ViewCompanyInvoices,
            IsotainerPermissions.Finance.GenerateTankInvoice,
            IsotainerPermissions.Finance.ViewInvoiceLines,
        ]);
        
        await RuntimeDataSeeder.SyncRolePermissionsAsync(userContext, IsotainerRoles.Washer, [
            IsotainerPermissions.Tank.ViewCompanies,
            IsotainerPermissions.Tank.ViewIsotainers,
            IsotainerPermissions.Tank.ViewWashStatuses,
            IsotainerPermissions.Wash.ViewWashTypes,
            IsotainerPermissions.Wash.ViewWashInstructions,
            IsotainerPermissions.Wash.UpdateWashInstruction,
        ]);
        
        await TankDataSeeder.SeedWashStatusAsync(tankContext);
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

app.MapRoleEndpoints();
app.MapPermissionEndpoints();
app.MapUserEndpoints();

app.MapCompanyEndpoints();
app.MapIsotainerTankEndpoints();
app.MapWashStatusEndpoints();

app.MapWashInstructionEndpoints();
app.MapWashTypeEndpoints();

app.UseAuthorization();

app.Run();