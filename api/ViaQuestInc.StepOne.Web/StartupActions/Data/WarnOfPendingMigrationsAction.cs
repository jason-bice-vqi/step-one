using Microsoft.EntityFrameworkCore;
using Serilog;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;
using Environments = ViaQuestInc.StepOne.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// An action that warns developers of pending migrations.
/// </summary>
public class WarnOfPendingMigrationsAction(StepOneDbContext dbContext, DatabaseConfig databaseConfig)
    : DatabaseStartupActionBase(dbContext, databaseConfig)
{
    protected override IEnumerable<DatabaseStartupTypes> RequiredByStartupTypes =>
        new[] { DatabaseStartupTypes.NoAction };

    protected override IEnumerable<string> SupportedEnvironmentNames => new[]
    {
        Environments.Local,
        Environments.Development,
        Environments.Production,
        Environments.Staging
    };

    public override async Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        await base.OnStartupAsync(app, cancellationToken);

        if (!databaseConfig.EnableMigrations)
        {
            Log.Information("    Migrations are disabled; exiting action.");
            
            return;
        }

        var pendingMigrations = (await DbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToArray();

        if (pendingMigrations.Length != 0)
        {
            Log.Warning(
                "    There are {PendingMigrationsCount} pending migrations that should be applied.",
                pendingMigrations.Length
            );
        }
        else
        {
            Log.Information("    {NoPendingMigrations} pending migrations", "0");
        }
    }
}