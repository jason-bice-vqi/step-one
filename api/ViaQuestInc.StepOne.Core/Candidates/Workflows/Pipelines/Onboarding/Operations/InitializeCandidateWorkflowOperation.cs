using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public class InitializeCandidateWorkflowOperation(CandidateWorkflowService candidateWorkflowService)
    : ICandidateOnboardingOperation
{
    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        var candidate = options.Request.Candidate!;
        var workflow = options.Request.Workflow!;

        await candidateWorkflowService.CreateAsync(candidate, options.Request.JobTitleId, workflow, cancellationToken);
    }
}