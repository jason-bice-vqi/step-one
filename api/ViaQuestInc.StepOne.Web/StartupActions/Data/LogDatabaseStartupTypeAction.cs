using Serilog;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// Logs information about how the database will be managed throughout the rest of the startup process.
/// </summary>
/// <remarks>This should not inherit from <see cref="DatabaseStartupActionBase"/>, as it applies to all environments
/// and all startup types.</remarks>
public class LogDatabaseStartupTypeAction(DatabaseConfig databaseConfig) : IStartupAction
{
    public Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        Log.Information("  Database.DatabaseStartupType: {StartupType}", databaseConfig.DatabaseStartupType);

        switch (databaseConfig.DatabaseStartupType)
        {
            case DatabaseStartupTypes.NoAction:
                Log.Information("  {Message}", "The database will not be modified during startup.");

                break;
            case DatabaseStartupTypes.Recreate:
                Log.Information(
                    "  {Message}",
                    "The database will be deleted, created (with EF Migrations support), and seeded."
                );

                break;
            case DatabaseStartupTypes.ApplyMigrations:
                Log.Information("  {Message}", "The database will have any pending EF Migrations applied.");

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(DatabaseConfig.DatabaseStartupType))
                {
                    HelpLink =
                        "https://bitbucket.org/atlanticcasualty/aces2-api/wiki/Developer%20Guidelines/Backend/Database%20Management"
                };
        }

        return Task.CompletedTask;
    }
}