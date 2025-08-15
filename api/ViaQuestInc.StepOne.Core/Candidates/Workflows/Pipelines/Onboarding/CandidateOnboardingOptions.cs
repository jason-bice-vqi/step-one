namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding;

public class CandidateOnboardingOptions(CandidateOnboardingRequest request)
{
    public CandidateOnboardingRequest Request { get; } = request;
}