using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Organization;
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

    public async Task CreateAsync(int? candidateId, int? jobTitleId, CancellationToken cancellationToken)
    {
        var candidates = await repository.FilterWithChildren<Candidate>(
                x => x.CandidateWorkflowId == null && 
                     (candidateId == null || x.Id == candidateId) &&
                     (jobTitleId == null || x.JobTitleId == jobTitleId) &&
                     x.JobTitleId != null &&
                     x.JobTitle!.WorkflowId != null,
                $"{nameof(Candidate.JobTitle)}.{nameof(JobTitle.Workflow)}.{nameof(Workflow.WorkflowSteps)}")
            .ToArrayAsync(cancellationToken);

        foreach (var candidate in candidates)
        {
            await CreateAsync(candidate, candidate.JobTitle!.Workflow!, cancellationToken);
        }
    }

    public async Task CreateAsync(
        Candidate candidate,
        Workflow workflow,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(workflow.WorkflowSteps);

        var candidateWorkflow = new CandidateWorkflow
        {
            Id = 0,
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
                        Id = 0,
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