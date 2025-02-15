using Microsoft.Extensions.Options;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;
using Environments = ViaQuestInc.StepOne.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// An action that deletes a database.
/// </summary>
public class DeleteDatabaseAction(StepOneDbContext dbContext, IOptions<DatabaseConfig> databaseConfigOptions)
    : DatabaseStartupActionBase(dbContext, databaseConfigOptions)
{
    protected override IEnumerable<DatabaseStartupTypes> RequiredByStartupTypes => new[]
    {
        DatabaseStartupTypes.Recreate
    };

    protected override IEnumerable<string> SupportedEnvironmentNames => new[]
    {
        Environments.Development,
        Environments.Local
    };

    public override async Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        await base.OnStartupAsync(app, cancellationToken);

        await DbContext.Database.EnsureDeletedAsync(cancellationToken);
    }
}