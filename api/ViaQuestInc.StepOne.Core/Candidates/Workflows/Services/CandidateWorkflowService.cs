using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Core.Workflows.Steps;
using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;

public class CandidateWorkflowService(IRepository repository)
{
    private static readonly string[] CandidateWorkflowIncludes =
    [
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.CandidateWorkflowSteps)}.{nameof(CandidateWorkflowStep.WorkflowStep)}.{nameof(WorkflowStep.Step)}",
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.Workflow)}"
    ];

    public async Task<CandidateWorkflow> CreateAsync(Candidate candidate, Workflow workflow,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(workflow.WorkflowSteps);

        var candidateWorkflow = new CandidateWorkflow
        {
            CandidateId = candidate.Id,
            WorkflowId = workflow.Id,
            CreatedAt = DateTime.UtcNow,
            EntityStatus = EntityStatuses.Active
        };

        await repository.CreateAsync(candidateWorkflow, cancellationToken);

        var candidateWorkflowSteps = workflow.WorkflowSteps.Select(x =>
            new CandidateWorkflowStep
            {
                CandidateWorkflowId = candidateWorkflow.Id,
                WorkflowStepId = x.Id
            }
        ).ToArray();

        await repository.CreateRangeAsync(candidateWorkflowSteps, cancellationToken);

        candidate.CandidateWorkflowId = candidateWorkflow.Id;

        await repository.UpdateAsync(candidate, cancellationToken);

        return (await GetAsync(candidate.Id, cancellationToken))!;
    }

    public async Task<CandidateWorkflow?> GetAsync(int candidateId, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<CandidateWorkflow>(x => x.CandidateId == candidateId, cancellationToken);
    }

    public async Task<Candidate?> GetWithWorkflowAsync(int candidateId, CancellationToken cancellationToken)
    {
        return await repository.GetWithChildrenAsync<Candidate>(x => x.Id == candidateId,
            cancellationToken, CandidateWorkflowIncludes);
    }
}