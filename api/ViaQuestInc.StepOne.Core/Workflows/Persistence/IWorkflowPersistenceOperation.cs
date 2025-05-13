namespace ViaQuestInc.StepOne.Core.Workflows.Persistence;

public interface IWorkflowPersistenceOperation
{
    Task<bool> ShouldExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken);

    Task ExecuteAsync(PipelineOptions pipelineOptions, CancellationToken cancellationToken);
}