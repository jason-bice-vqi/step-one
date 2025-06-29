using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Workflows.Persistence.Operations;

/// <summary>
/// An operation that deletes old workflow steps that are no longer included on the workflow.
/// </summary>
public class DeleteWorkflowStepsOperation(IRepository<StepOneDbContext> repository) : IWorkflowPersistenceOperation
{
    public Task<bool> ShouldExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        return Task.FromResult(pipelineOptions.OriginalWorkflow.WorkflowSteps.Count > 0);
    }

    public async Task ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        var stepsToRemove = pipelineOptions.OriginalWorkflow.WorkflowSteps
            .Except<WorkflowStep>(pipelineOptions.UpdatedWorkflow.WorkflowSteps, new EntityIdEqualityComparer<int>())
            .ToArray();

        await repository.DeleteRangeAsync(stepsToRemove, cancellationToken);
    }
}