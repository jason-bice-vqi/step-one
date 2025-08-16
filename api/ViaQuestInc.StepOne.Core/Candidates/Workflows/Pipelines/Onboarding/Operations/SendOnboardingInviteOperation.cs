using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public class SendOnboardingInviteOperation(CandidateWorkflowService candidateWorkflowService)
    : ICandidateOnboardingOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        return Task.FromResult(options.Request.SendOnboardingInvite);
    }

    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        await candidateWorkflowService.SendInviteAsync(options.Request.Candidate!, cancellationToken);
    }
}