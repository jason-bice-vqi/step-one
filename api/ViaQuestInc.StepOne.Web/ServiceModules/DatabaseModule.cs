using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Organization.HrTracker;
using ViaQuestInc.StepOne.Core.Organization.Synchronizers;
using ViaQuestInc.StepOne.Web.StartupActions;
using ViaQuestInc.StepOne.Web.StartupActions.Data;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class DatabaseModule : IServiceModule
{
    public string ConnectionString { get; set; }

    public string HrtConnectionString { get; set; }

    public int CommandTimeout { get; set; } = 120;

    public bool EnableSensitiveDataLogging { get; set; }

    public void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env)
    {
        Log.Information("  Registering database config");
        var databaseConfig = configuration.GetSection("Database")
                                 .Get<DatabaseConfig>() ??
                             throw new InvalidOperationException("Database configuration is missing");

        services.AddSingleton(databaseConfig);

        Log.Information("  Registering StepOne database services");
        services.AddScoped<IRepository<StepOneDbContext>, Repository<StepOneDbContext>>()
            // Database start-up actions
            .AddTransient<IStartupAction, LogDatabaseStartupTypeAction>()
            .AddTransient<IStartupAction, DeleteDatabaseAction>()
            .AddTransient<IStartupAction, CreateDatabaseAction>()
            .AddTransient<IStartupAction, ApplyMigrationsAction>()
            .AddTransient<IStartupAction, PopulateDatabaseAction>()
            .AddTransient<IStartupAction, SynchronizeWithHrtAction>()
            .AddTransient<IStartupAction, WarnOfPendingMigrationsAction>(); // Must be last AcesDbContext action

        Log.Information("  Registering StepOne database context");
        services.AddHealthChecks()
            .Services.AddSqlServer<StepOneDbContext>(
                ConnectionString,
                o => o.MigrationsAssembly("ViaQuestInc.StepOne.Infrastructure")
                    .CommandTimeout(CommandTimeout),
                o => o
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS))
                    .EnableSensitiveDataLogging(EnableSensitiveDataLogging));

        Log.Information("  Registering HR Tracker database services");
        services.AddScoped<IRepository<HrtDbContext>, Repository<HrtDbContext>>();

        Log.Information("  Registering HR Tracker database context");
        services.AddHealthChecks()
            .Services.AddSqlServer<HrtDbContext>(
                HrtConnectionString,
                null,
                o => o
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS))
                    .EnableSensitiveDataLogging(EnableSensitiveDataLogging));

        Log.Information("  Registering HRT-to-StepOne Synchronizers");
        services.AddTransient<IDataSynchronizer, HrtSynchronizer<HrtCompany, Company, int>>();
        services.AddTransient<IDataSynchronizer, HrtSynchronizer<HrtRegion, Region, int>>();
        services.AddTransient<IDataSynchronizer, HrtSynchronizer<HrtBranch, Branch, int>>();
        // Requires Companies to be current:
        services.AddTransient<IDataSynchronizer, HrtSynchronizer<HrtJobTitle, JobTitle, int>>();
    }
}