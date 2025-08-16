using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Organization.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public class AssignWorkflowToJobTitleOperation(JobTitleService jobTitleService) : ICandidateOnboardingOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        return Task.FromResult(options.Request.AssignWorkflowToJobTitle);
    }

    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        var jobTitle = options.Request.JobTitle!;

        var updateJobTitleRequest = new UpdateJobTitleRequest
        {
            WorkflowId = options.Request.WorkflowId
        };
        
        await jobTitleService.UpdateAsync(jobTitle, updateJobTitleRequest, cancellationToken);

        options.Request.JobTitle = await jobTitleService.ShowAsync(options.Request.JobTitleId, cancellationToken);
    }
}