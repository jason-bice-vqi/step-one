namespace ViaQuestInc.StepOne.Core.Workflows.Persistence.Operations;

/// <summary>
/// An operation that resequences the steps within the workflow based on their order upon arrival. 
/// </summary>
public class ResequenceWorkflowStepsOperation : IWorkflowPersistenceOperation
{
    public Task<bool> ShouldExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public Task ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        for (var i = 0; i < pipelineOptions.UpdatedWorkflow.WorkflowSteps.Count; i++)
            pipelineOptions.UpdatedWorkflow.WorkflowSteps.ElementAt(i)
                .StepIndex = i;

        return Task.CompletedTask;
    }
}