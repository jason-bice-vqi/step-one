using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Workflows;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

public class InitializeRequestOperation(IRepository<StepOneDbContext> repository) : ICandidateOnboardingOperation
{
    public async Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        options.Request.Candidate =
            await repository.FindAsync<Candidate>(options.Request.CandidateId, cancellationToken);

        options.Request.JobTitle = await repository.FindAsync<JobTitle>(options.Request.JobTitleId, cancellationToken);

        options.Request.Workflow = await repository.FindAsync<Workflow>(options.Request.WorkflowId, cancellationToken);
    }
}