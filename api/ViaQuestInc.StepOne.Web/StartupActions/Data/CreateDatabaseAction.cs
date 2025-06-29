using Microsoft.EntityFrameworkCore;
using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using Environments = ViaQuestInc.StepOne.Core.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// An action that creates a database with support for EF Migrations.
/// </summary>
public class CreateDatabaseAction(StepOneDbContext dbContext, DatabaseConfig databaseConfig)
    : DatabaseStartupActionBase(dbContext, databaseConfig)
{
    protected override IEnumerable<DatabaseStartupTypes> RequiredByStartupTypes => new[]
    {
        DatabaseStartupTypes.Recreate
    };

    public override async Task<bool> ShouldExecuteAsync(WebApplication app, CancellationToken cancellationToken)
    {
        // Attempt to connect to the database to determine whether it exists. If it does, this action should not run.
        // The database command to create the database with support for migrations is the same command used to apply
        // pending migrations. If this action were to run against an existing database, its migrations would get
        // automatically applied.
        DatabaseStartupStatus.IsNew = !await DbContext.Database.CanConnectAsync(cancellationToken);

        return await base.ShouldExecuteAsync(app, cancellationToken) || DatabaseStartupStatus.IsNew;
    }

    protected override IEnumerable<string> SupportedEnvironmentNames => new[]
    {
        Environments.Development,
        Environments.Local,
        Environments.Production,
        Environments.Staging
    };

    public override async Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        await base.OnStartupAsync(app, cancellationToken);

        if (databaseConfig.EnableMigrations)
        {
            var pendingMigrations = await DbContext.Database.GetPendingMigrationsAsync(cancellationToken);

            Log.Information("  Applying {Migrations} pending EF Migration(s).", pendingMigrations.Count());

            await DbContext.Database.MigrateAsync(cancellationToken);

            return;
        }

        // Migrations disabled; just recreate from scratch.
        Log.Information("  Migrations disabled; recreating from context schema.");
        await DbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}