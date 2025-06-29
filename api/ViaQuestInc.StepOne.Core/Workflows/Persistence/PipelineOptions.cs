namespace ViaQuestInc.StepOne.Core.Workflows.Persistence;

public class PipelineOptions(Workflow originalWorkflow, Workflow updatedWorkflow)
{
    public Workflow OriginalWorkflow { get; private set; } = originalWorkflow;

    public Workflow UpdatedWorkflow { get; private set; } = updatedWorkflow;
}