namespace ViaQuestInc.StepOne.Core.Data.Entity;

public interface IDataSynchronizer
{
    /// <summary>
    /// Determines whether the synchronizer should execute.
    /// </summary>
    /// <returns>
    /// An awaitable task containing the result, where true indicates the synchronizer should run and false
    /// indicates it should not.
    /// </returns>
    Task<bool> ShouldSyncAsync(
        IRepository<StepOneDbContext> stepOneRepository,
        IRepository<HrtDbContext> hrtRepository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Executes the synchronizer.
    /// </summary>
    Task SyncAsync(
        IRepository<StepOneDbContext> stepOneRepository,
        IRepository<HrtDbContext> hrtRepository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken);
}