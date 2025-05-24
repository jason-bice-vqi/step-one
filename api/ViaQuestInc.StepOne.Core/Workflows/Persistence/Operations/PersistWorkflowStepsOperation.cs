using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Persistence.Operations;

/// <summary>
/// An operation that persists the steps included on the workflow.
/// </summary>
public class PersistWorkflowStepsOperation(IRepository repository) : IWorkflowPersistenceOperation
{
    public Task<bool> ShouldExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public async Task ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        var workflowSteps = pipelineOptions.UpdatedWorkflow.WorkflowSteps.ToArray();
        
        await repository.UpdateRangeAsync(workflowSteps, cancellationToken);
    }
}