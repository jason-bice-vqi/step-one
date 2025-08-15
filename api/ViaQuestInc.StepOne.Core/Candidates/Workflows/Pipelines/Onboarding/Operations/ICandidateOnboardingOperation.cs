namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public interface ICandidateOnboardingOperation
{
    Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken) =>
        Task.FromResult(true);

    Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken);
}