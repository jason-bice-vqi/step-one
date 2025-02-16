using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        services.AddScoped<IRepository, Repository<StepOneDbContext>>()
            // Database start-up actions
            .AddTransient<IStartupAction, LogDatabaseStartupTypeAction>()
            .AddTransient<IStartupAction, DeleteDatabaseAction>()
            .AddTransient<IStartupAction, CreateDatabaseAction>()
            .AddTransient<IStartupAction, ApplyMigrationsAction>()
            .AddTransient<IStartupAction, PopulateDatabaseAction>()
            .AddTransient<IStartupAction, WarnOfPendingMigrationsAction>(); // Must be last AcesDbContext action
        
        services.AddHealthChecks().Services.AddSqlServer<StepOneDbContext>(ConnectionString,
            o => o.MigrationsAssembly("ViaQuestInc.StepOne.Infrastructure").CommandTimeout(CommandTimeout),
            o => o
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS))
                .EnableSensitiveDataLogging(EnableSensitiveDataLogging));
    }
}