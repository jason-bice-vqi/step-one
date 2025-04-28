using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Web.StartupActions;
using ViaQuestInc.StepOne.Web.StartupActions.Data;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class DatabaseModule : IServiceModule
{
    public string ConnectionString { get; set; }

    public int CommandTimeout { get; set; } = 120;

    public bool EnableSensitiveDataLogging { get; set; }

    public void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env)
    {
        Log.Information("  Registering database config");
        var databaseConfig = configuration.GetSection("Database").Get<DatabaseConfig>() ??
                         throw new InvalidOperationException("Database configuration is missing");
        
        services.AddSingleton(databaseConfig);
        
        Log.Information("  Registering database services");
        services.AddScoped<IRepository, Repository<StepOneDbContext>>()
            // Database start-up actions
            .AddTransient<IStartupAction, LogDatabaseStartupTypeAction>()
            .AddTransient<IStartupAction, DeleteDatabaseAction>()
            .AddTransient<IStartupAction, CreateDatabaseAction>()
            .AddTransient<IStartupAction, ApplyMigrationsAction>()
            .AddTransient<IStartupAction, PopulateDatabaseAction>()
            .AddTransient<IStartupAction, WarnOfPendingMigrationsAction>(); // Must be last AcesDbContext action
        
        Log.Information("  Registering database context");
        services.AddHealthChecks().Services.AddSqlServer<StepOneDbContext>(ConnectionString,
            o => o.MigrationsAssembly("ViaQuestInc.StepOne.Infrastructure").CommandTimeout(CommandTimeout),
            o => o
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS))
                .EnableSensitiveDataLogging(EnableSensitiveDataLogging));
    }
}