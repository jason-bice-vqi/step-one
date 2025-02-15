using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;
using Environments = ViaQuestInc.StepOne.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// An action that automatically applies EF Migrations to a database.
/// </summary>
public class ApplyMigrationsAction(StepOneDbContext dbContext, IOptions<DatabaseConfig> databaseConfigOptions)
    : DatabaseStartupActionBase(dbContext, databaseConfigOptions)
{
    protected override IEnumerable<DatabaseStartupTypes> RequiredByStartupTypes => new[]
    {
        DatabaseStartupTypes.ApplyMigrations
    };

    protected override IEnumerable<string> SupportedEnvironmentNames => new[]
    {
        Environments.Local
    };

    public override async Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        await base.OnStartupAsync(app, cancellationToken);

        if (!databaseConfigOptions.Value.EnableMigrations)
        {
            throw new Exception("Cannot apply migrations with migrations disabled.");
        }

        var pendingMigrations = await DbContext.Database.GetPendingMigrationsAsync(cancellationToken);

        Log.Information("    Applying {Migrations} pending EF Migration(s)", pendingMigrations.Count());

        await DbContext.Database.MigrateAsync(cancellationToken);
    }
}