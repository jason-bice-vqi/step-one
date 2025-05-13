using ViaQuestInc.StepOne.Core.Workflows.Services;

namespace ViaQuestInc.StepOne.Core.Workflows.Persistence;

public class WorkflowPersistenceEngine(
    IEnumerable<IWorkflowPersistenceOperation> workflowPersistenceOperations,
    WorkflowService workflowService)
{
    public async Task<Workflow> ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken)
    {
        foreach (var operation in workflowPersistenceOperations)
        {
            if (!await operation.ShouldExecuteAsync(pipelineOptions, cancellationToken)) continue;

            await operation.ExecuteAsync(pipelineOptions, cancellationToken);
        }

        return await workflowService.GetAsync(pipelineOptions.UpdatedWorkflow.Id, cancellationToken);
    }
}