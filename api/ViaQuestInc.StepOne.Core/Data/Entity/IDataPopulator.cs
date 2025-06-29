namespace ViaQuestInc.StepOne.Core.Data.Entity;

public interface IDataPopulator
{
    /// <summary>
    /// Determines whether the populator should execute.
    /// </summary>
    /// <returns>
    /// An awaitable task containing the result, where true indicates the populator should run and false
    /// indicates it should not.
    /// </returns>
    Task<bool> ShouldExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Executes the populator.
    /// </summary>
    Task PopulateAsync(
        IRepository<StepOneDbContext> repository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken);
}