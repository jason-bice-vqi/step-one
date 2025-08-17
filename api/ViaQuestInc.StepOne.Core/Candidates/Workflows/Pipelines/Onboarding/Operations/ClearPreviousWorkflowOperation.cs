using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

/// <summary>
/// An operation that resets a candidate's workflow/onboarding progress in the event they've been assigned to a new
/// workflow after having been previously assigned to a different one.
/// </summary>
public class ClearPreviousWorkflowOperation(CandidateWorkflowService candidateWorkflowService)
    : ICandidateOnboardingOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        return Task.FromResult(options.Request.Candidate!.CandidateWorkflowId != null);
    }

    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        await candidateWorkflowService.DeleteAsync(options.Request.CandidateId!.Value, cancellationToken);
    }
}