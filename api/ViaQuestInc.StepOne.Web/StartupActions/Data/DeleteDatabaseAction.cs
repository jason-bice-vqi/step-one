﻿using ViaQuestInc.StepOne.Core.Data;
using Environments = ViaQuestInc.StepOne.Core.Kernel.Environments;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

/// <summary>
/// An action that deletes a database.
/// </summary>
public class DeleteDatabaseAction(StepOneDbContext dbContext, DatabaseConfig databaseConfig)
    : DatabaseStartupActionBase(dbContext, databaseConfig)
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