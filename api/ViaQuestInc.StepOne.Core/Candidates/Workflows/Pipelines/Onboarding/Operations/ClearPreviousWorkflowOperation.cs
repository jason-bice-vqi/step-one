using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

/// <summary>
/// An operation that resets a candidate's workflow/onboarding progress in the event they've been assigned to a new
/// workflow after having been previously assigned to a different one.
/// </summary>
public class ClearPreviousWorkflowOperation(
    CandidateWorkflowService candidateWorkflowService,
    IRepository<StepOneDbContext> repository
)
    : ICandidateOnboardingOperation
{
    public async Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        // Get the candidate directly from the repository as cheaply as possible; the objects on the request haven't 
        // been initialized yet, but we just need to know if the candidate already has an assigned workflow.
        var candidate = (await repository.FindAsync<Candidate>(options.Request.CandidateId!, cancellationToken))!;

        return candidate.CandidateWorkflowId != null;
    }

    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        await candidateWorkflowService.DeleteAsync(options.Request.CandidateId!.Value, cancellationToken);
    }
}