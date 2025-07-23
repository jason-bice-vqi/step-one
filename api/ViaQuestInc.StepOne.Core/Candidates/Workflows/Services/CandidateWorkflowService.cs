using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;

public class CandidateWorkflowService(IRepository<StepOneDbContext> repository)
{
    private static readonly string[] CandidateWorkflowIncludes =
    [
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.CandidateWorkflowSteps)}.{nameof(CandidateWorkflowStep.WorkflowStep)}.{nameof(WorkflowStep.Step)}",
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.Workflow)}"
    ];

    public async Task<CandidateWorkflow?> AssignDefaultWorkflow(int candidateId, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetWithChildrenAsync<Candidate>(
            x => x.Id == candidateId,
            cancellationToken,
            nameof(Candidate.JobTitle));

        if (candidate?.JobTitle.WorkflowId == null) return null;

        var candidateWorkflow = new CandidateWorkflow
        {
            CandidateId = candidateId,
            WorkflowId = candidate!.JobTitle.WorkflowId!.Value,
            CreatedAt = DateTime.UtcNow,
            EntityStatus = EntityStatuses.Active,
            Id = 0
        };

        await repository.CreateAsync(candidateWorkflow, cancellationToken);
        
        

        return candidateWorkflow;
    }

    public async Task CreateAsync(
        Candidate candidate,
        Workflow workflow,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(workflow.WorkflowSteps);

        var candidateWorkflow = new CandidateWorkflow
        {
            Id = default,
            CandidateId = candidate.Id,
            WorkflowId = workflow.Id,
            CreatedAt = DateTime.UtcNow,
            EntityStatus = EntityStatuses.Active
        };

        await repository.CreateAsync(candidateWorkflow, cancellationToken);

        var candidateWorkflowSteps = workflow.WorkflowSteps.Select(
                x =>
                    new CandidateWorkflowStep
                    {
                        Id = default,
                        CandidateWorkflowId = candidateWorkflow.Id,
                        WorkflowStepId = x.Id
                    }
            )
            .ToArray();

        await repository.CreateRangeAsync(candidateWorkflowSteps, cancellationToken);

        candidate.CandidateWorkflowId = candidateWorkflow.Id;

        await repository.UpdateAsync(candidate, cancellationToken);
    }

    public async Task<CandidateWorkflow?> GetAsync(int candidateId, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<CandidateWorkflow>(x => x.CandidateId == candidateId, cancellationToken);
    }

    public async Task<Candidate?> GetWithWorkflowAsync(int candidateId, CancellationToken cancellationToken)
    {
        return await repository.GetWithChildrenAsync<Candidate>(
            x => x.Id == candidateId,
            cancellationToken,
            CandidateWorkflowIncludes);
    }
}