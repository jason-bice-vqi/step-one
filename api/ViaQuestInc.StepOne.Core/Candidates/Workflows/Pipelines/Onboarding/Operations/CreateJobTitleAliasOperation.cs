using ViaQuestInc.StepOne.Core.Organization.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public class CreateJobTitleAliasOperation(JobTitleAliasService jobTitleAliasService) : ICandidateOnboardingOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        var aliasExists = options.Request.JobTitle!.JobTitleAliases.Any(x => x.Alias.Equals(
            options.Request.Candidate!.AtsJobTitle,
            StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(options.Request.CreateJobTitleAlias && !aliasExists);
    }

    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        var jobTitleId = options.Request.JobTitleId;
        var alias = options.Request.Candidate!.AtsJobTitle;

        await jobTitleAliasService.CreateAsync(jobTitleId, alias, cancellationToken);
    }
}