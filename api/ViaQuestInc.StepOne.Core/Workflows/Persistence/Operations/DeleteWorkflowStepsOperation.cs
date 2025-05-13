using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Persistence.Operations;

/// <summary>
/// An operation that clears the old workflow steps from the workflow.
/// </summary>
public class DeleteWorkflowStepsOperation(IRepository repository) : IWorkflowPersistenceOperation
{
    public Task<bool> ShouldExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        return Task.FromResult(pipelineOptions.OriginalWorkflow.WorkflowSteps!.Count > 0);
    }

    public async Task ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        await repository.DeleteRangeAsync(pipelineOptions.OriginalWorkflow.WorkflowSteps!.ToList(), cancellationToken);
    }
}