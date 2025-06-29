using Microsoft.Extensions.Options;
using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

public class SynchronizeWithHrtAction(
    IRepository<StepOneDbContext> stepOneRepository,
    IRepository<HrtDbContext> hrtRepository,
    IServiceProvider serviceProvider,
    IEnumerable<IDataSynchronizer> dataSynchronizers
) : IStartupAction
{
    public async Task OnStartupAsync(WebApplication webApplication, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        
        var batchSize = scope.ServiceProvider
            .GetRequiredService<IOptions<DatabaseConfig>>()
            .Value.RowValueExpressionLimit;

        foreach (var dataSynchronizer in dataSynchronizers)
        {
            var shouldSync = await dataSynchronizer.ShouldSyncAsync(
                stepOneRepository,
                hrtRepository,
                serviceProvider,
                batchSize,
                cancellationToken);

            if (!shouldSync) continue;

            Log.Information("    Executing {SyncName}", dataSynchronizer.GetType().Name);
            
            await dataSynchronizer.SyncAsync(
                stepOneRepository,
                hrtRepository,
                serviceProvider,
                batchSize,
                cancellationToken);
        }
    }
}