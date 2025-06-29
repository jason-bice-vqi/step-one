using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Kernel.Entity;

public interface IDataSynchronizer
{
    /// <summary>
    /// Determines whether the synchronizer should execute.
    /// </summary>
    /// <returns>
    /// An awaitable task containing the result, where true indicates the synchronizer should run and false
    /// indicates it should not.
    /// </returns>
    Task<bool> ShouldSyncAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Executes the synchronizer.
    /// </summary>
    Task SyncAsync(
        IRepository repository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken);
}