using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding;

public class CandidateOnboardingEngine(
    CandidateService candidateService,
    IEnumerable<ICandidateOnboardingOperation> candidateOnboardingOperations
)
{
    public async Task<Candidate> ExecuteAsync(
        CandidateOnboardingRequest candidateOnboardingRequest,
        CancellationToken cancellationToken)
    {
        var options = new CandidateOnboardingOptions(candidateOnboardingRequest);
        
        foreach (var operation in candidateOnboardingOperations)
        {
            if (!await operation.ShouldExecuteAsync(options, cancellationToken)) continue;

            await operation.ExecuteAsync(options, cancellationToken);
        }
        
        return await candidateService.ShowAsync(candidateOnboardingRequest.CandidateId!.Value, cancellationToken);
    }
}