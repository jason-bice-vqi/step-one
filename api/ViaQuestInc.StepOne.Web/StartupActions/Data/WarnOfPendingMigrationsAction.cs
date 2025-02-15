using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;
using Environments = ViaQuestInc.StepOne.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// An action that warns developers of pending migrations.
/// </summary>
public class WarnOfPendingMigrationsAction(StepOneDbContext dbContext, IOptions<DatabaseConfig> databaseConfigOptions)
    : DatabaseStartupActionBase(dbContext, databaseConfigOptions)
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

        if (!databaseConfigOptions.Value.EnableMigrations)
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