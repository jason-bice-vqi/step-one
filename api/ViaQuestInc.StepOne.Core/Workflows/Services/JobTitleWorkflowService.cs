using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Services;

public class JobTitleWorkflowService(IRepository<StepOneDbContext> repository)
{
    public async Task<JobTitleWorkflow?> GetByJobTitleIdAsync(int jobTitleId, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<JobTitleWorkflow>(x => x.JobTitleId == jobTitleId, cancellationToken);
    }

    public async Task<JobTitleWorkflow> CreateAsync(int jobTitleId, int workflowId, CancellationToken cancellationToken)
    {
        await DeleteByJobTitleAsync(jobTitleId, cancellationToken);
        
        var jobTitleWorkflow = new JobTitleWorkflow
        {
            JobTitleId = jobTitleId,
            WorkflowId = workflowId,
            Id = 0
        };

        return await repository.CreateAsync(jobTitleWorkflow, cancellationToken);
    }

    public async Task DeleteByJobTitleAsync(int jobTitleId, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync<JobTitleWorkflow>(x => x.JobTitleId == jobTitleId, cancellationToken);
    }
}