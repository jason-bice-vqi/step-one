using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Workflows.Steps;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Services;

public class WorkflowService(IRepository repository)
{
    private static readonly string[] DefaultIncludes =
    [
        nameof(Workflow.WorkflowSteps),
        $"{nameof(Workflow.WorkflowSteps)}.{nameof(WorkflowStep.Step)}"
    ];
    
    public async Task<IEnumerable<Workflow>> GetAsync(CancellationToken cancellationToken)
    {
        return await repository.AllWithChildren<Workflow>(DefaultIncludes).ToArrayAsync(cancellationToken);
    }
    
    public async Task<Workflow> GetAsync(int workflowId, CancellationToken cancellationToken)
    {
        return (await repository.GetWithChildrenAsync<Workflow>(x => x.Id == workflowId, cancellationToken,
            DefaultIncludes))!;
    }

    public async Task<Workflow> GetWithJobTitleWorkflowsAsync(int workflowId, CancellationToken cancellationToken)
    {
        var includes = DefaultIncludes.Concat(new [] {nameof(Workflow.JobTitleWorkflows)}).ToArray();

        return (await repository.GetWithChildrenAsync<Workflow>(x => x.Id == workflowId, cancellationToken, includes))!;
    }
}