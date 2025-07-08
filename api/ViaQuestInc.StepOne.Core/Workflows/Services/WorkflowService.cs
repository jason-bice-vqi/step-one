using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Workflows.Services;

public class WorkflowService(IRepository<StepOneDbContext> repository)
{
    private static readonly string[] DefaultIncludes =
    [
        nameof(Workflow.WorkflowSteps),
        $"{nameof(Workflow.WorkflowSteps)}.{nameof(WorkflowStep.Step)}"
    ];

    public async Task<Workflow> CreateAsync(Workflow workflow, CancellationToken cancellationToken)
    {
        var createdWorkflow = await repository.CreateAsync(workflow, cancellationToken);

        if (workflow.CopiedFromWorkflowId == null) return createdWorkflow;

        var copiedFromWorkflow = await ShowAsync(workflow.CopiedFromWorkflowId.Value, cancellationToken);

        if (workflow.CopySteps)
        {
            var workflowSteps = copiedFromWorkflow.WorkflowSteps.ToArray();

            foreach (var workflowStep in workflowSteps)
            {
                workflowStep.WorkflowId = createdWorkflow.Id;
                workflowStep.Id = 0;
            }

            await repository.CreateRangeAsync(workflowSteps, cancellationToken);
        }

        return createdWorkflow;
    }

    public async Task<IEnumerable<Workflow>> IndexAsync(CancellationToken cancellationToken)
    {
        return await repository.AllWithChildren<Workflow>(DefaultIncludes)
            .OrderBy(x => x.Name)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Workflow> ShowAsync(int workflowId, CancellationToken cancellationToken)
    {
        return (await repository.GetWithChildrenAsync<Workflow>(
            x => x.Id == workflowId,
            cancellationToken,
            DefaultIncludes))!;
    }

    public async Task<int> DeleteAsync(Workflow workflow, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(workflow, cancellationToken);
    }
}