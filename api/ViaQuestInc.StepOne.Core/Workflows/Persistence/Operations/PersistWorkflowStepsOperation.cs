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
        foreach (var workflowStep in pipelineOptions.UpdatedWorkflow.WorkflowSteps!)
        {
            workflowStep.Id = default;
        }
        
        await repository.CreateRangeAsync(pipelineOptions.UpdatedWorkflow.WorkflowSteps.ToList(), cancellationToken);
    }
}