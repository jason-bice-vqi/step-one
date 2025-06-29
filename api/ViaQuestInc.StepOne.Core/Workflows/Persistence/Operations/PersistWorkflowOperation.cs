using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Persistence.Operations;

/// <summary>
/// An operation that persists the workflow, presumably after all nested / navigation properties have been handled.
/// </summary>
public class PersistWorkflowOperation(IRepository<StepOneDbContext> repository) : IWorkflowPersistenceOperation
{
    public Task<bool> ShouldExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public async Task ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        await repository.UpdateAsync(pipelineOptions.UpdatedWorkflow, cancellationToken);
    }
}