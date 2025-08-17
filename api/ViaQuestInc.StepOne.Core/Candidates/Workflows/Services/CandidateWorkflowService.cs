using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;

public class CandidateWorkflowService(IRepository<StepOneDbContext> repository)
{
    private static readonly string[] DefaultIncludes =
    [
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.CandidateWorkflowSteps)}.{nameof(CandidateWorkflowStep.WorkflowStep)}.{nameof(WorkflowStep.Step)}",
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.Workflow)}"
    ];

    public async Task CreateAsync(
        Candidate candidate,
        int jobTitleId,
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
        candidate.CandidateWorkflowStatus = CandidateWorkflowStatus.NotInvited;
        candidate.JobTitleId = jobTitleId;

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
            DefaultIncludes);
    }

    public async Task SendInviteAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        // Only update the workflow status if this is the initial invitation. Follow-up reminders may be issued by users
        // after the candidate has started their workflow and have a "downstream status" already assigned.
        if (candidate.CandidateWorkflowStatus == CandidateWorkflowStatus.Unassigned)
        {
            candidate.CandidateWorkflowStatus = CandidateWorkflowStatus.NotStarted;
            
            await repository.UpdateAsync(candidate, cancellationToken);
        }
        
        // TODO
        Log.Error(
            "{method} not yet implemented. Need to determine onboarding invite mechanism.",
            nameof(SendInviteAsync));
    }

    public async Task DeleteAsync(int candidateId, CancellationToken cancellationToken)
    {
        var candidate = (await repository.FindAsync<Candidate>(candidateId, cancellationToken))!;

        if (candidate.CandidateWorkflowId == null) return;

        await repository.DeleteAsync<CandidateWorkflow>(x => x.Id == candidate.CandidateWorkflowId, cancellationToken);
    }
}