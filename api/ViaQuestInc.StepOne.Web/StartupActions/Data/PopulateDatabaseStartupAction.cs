using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;
using Environments = ViaQuestInc.StepOne.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;
using Environments = Environments;

/// <summary>
/// An action that seeds the database with the required initial data.
/// </summary>
public class PopulateDatabaseAction(StepOneDbContext dbContext, DatabaseConfig databaseConfig)
    : DatabaseStartupActionBase(dbContext, databaseConfig)
{
    protected override IEnumerable<DatabaseStartupTypes> RequiredByStartupTypes => new[]
    {
        DatabaseStartupTypes.Recreate
    };

    protected override IEnumerable<string> SupportedEnvironmentNames => new[]
    {
        Environments.Development,
        Environments.Local,
        Environments.Production,
        Environments.Staging
    };

    public override async Task<bool> ShouldExecuteAsync(WebApplication app, CancellationToken cancellationToken)
    {
        return await base.ShouldExecuteAsync(app, cancellationToken) || DatabaseStartupStatus.IsNew;
    }

    public override async Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        await base.OnStartupAsync(app, cancellationToken);

        if (DatabaseStartupStatus.IsNew && DatabaseConfig.DatabaseStartupType != DatabaseStartupTypes.Recreate)
        {
            Log.Warning(
                "  Database startup type is {startupType}, but database is new. Running populators.",
                DatabaseConfig.DatabaseStartupType
            );
        }

        if (DatabaseConfig.DisablePopulators)
        {
            Log.Warning("  Database populators disabled");

            return;
        }

        await StepOneDbContext.PopulateDatabaseAsync(app.Services);
    }
}