using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Core.Organization.Services;
using ViaQuestInc.StepOne.Core.Workflows.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public class InitializeRequestOperation(
    CandidateService candidateService,
    JobTitleService jobTitleService,
    WorkflowService workflowService
) : ICandidateOnboardingOperation
{
    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        options.Request.Candidate = await candidateService.ShowAsync(
            options.Request.CandidateId!.Value,
            cancellationToken);
        
        options.Request.JobTitle = await jobTitleService.ShowAsync(options.Request.JobTitleId, cancellationToken);
        
        options.Request.Workflow = await workflowService.ShowAsync(options.Request.WorkflowId, cancellationToken);
    }
}