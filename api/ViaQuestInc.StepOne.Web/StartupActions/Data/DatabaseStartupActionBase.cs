using Serilog;
using ViaQuestInc.StepOne.Infrastructure.Data;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

public abstract class DatabaseStartupActionBase(
    StepOneDbContext dbContext,
    DatabaseConfig databaseConfig)
    : IStartupAction
{
    protected readonly StepOneDbContext DbContext = dbContext;
    protected readonly DatabaseConfig DatabaseConfig = databaseConfig;

    protected abstract IEnumerable<DatabaseStartupTypes> RequiredByStartupTypes { get; }
    protected abstract IEnumerable<string> SupportedEnvironmentNames { get; }

    public virtual Task<bool> ShouldExecuteAsync(WebApplication app, CancellationToken cancellationToken) =>
        Task.FromResult(RequiredByStartupTypes.Contains(DatabaseConfig.DatabaseStartupType));

    public virtual Task OnStartupAsync(WebApplication app, CancellationToken cancellationToken = default)
    {
        const string startupTypeName = nameof(DatabaseConfig.DatabaseStartupType);
        var startupType = DatabaseConfig.DatabaseStartupType;
        var environmentName = app.Environment.EnvironmentName;

        if (SupportedEnvironmentNames.Contains(environmentName)) return Task.CompletedTask;

        Log.Fatal(
            "  Invalid {StartupType}: {StartupTypeValue} is prohibited in the {Environment} environment. See {DatabaseManagementReference}",
            startupTypeName,
            startupType,
            environmentName,
            "https://bitbucket.org/atlanticcasualty/aces2.0/wiki/Developer%20Guidelines/Backend/Database%20Management"
        );

        throw new($"Invalid {startupTypeName}: {startupType} is forbidden in environment {environmentName}");
    }
}