using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Services;

public class JobTitleService(IRepository<StepOneDbContext> repository)
{
    private readonly string[] defaultIncludes = [nameof(JobTitle.Company), nameof(JobTitle.Workflow)];
    
    public async Task<IEnumerable<JobTitle>> IndexAsync(CancellationToken cancellationToken)
    {
        return await repository.FilterWithChildren<JobTitle>(
                x => x.EntityStatus == EntityStatuses.Active,
                defaultIncludes)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<JobTitle>> IndexAsync(int companyId, CancellationToken cancellationToken)
    {
        return await repository
            .FilterWithChildren<JobTitle>(
                x => x.CompanyId == companyId && x.EntityStatus == EntityStatuses.Active,
                defaultIncludes)
            .OrderBy(x => x.Title)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<JobTitle?> GetAsync(int jobTitleId, CancellationToken cancellationToken)
    {
        return await repository.FindAsync<JobTitle>(jobTitleId, cancellationToken);
    }

    public async Task UpdateAsync(
        JobTitle jobTitle,
        UpdateJobTitleRequest updateJobTitleRequest,
        CancellationToken cancellationToken)
    {
        jobTitle.WorkflowId = updateJobTitleRequest.WorkflowId;

        await repository.UpdateAsync(jobTitle, cancellationToken);
    }
}