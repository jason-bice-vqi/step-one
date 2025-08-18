using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Services;

public class JobTitleService(IRepository<StepOneDbContext> repository)
{
    private static readonly string[] DefaultIncludes =
    [
        nameof(JobTitle.Company),
        nameof(JobTitle.JobTitleAliases),
        nameof(JobTitle.Workflow)
    ];

    public async Task<IEnumerable<JobTitle>> IndexAsync(CancellationToken cancellationToken)
    {
        return await IndexAsync(null, cancellationToken);
    }

    public async Task<IEnumerable<JobTitle>> IndexAsync(int? companyId, CancellationToken cancellationToken)
    {
        return (await repository
            .FilterWithChildren<JobTitle>(
                x => (companyId == null || x.CompanyId == companyId) && x.EntityStatus == EntityStatuses.Active,
                DefaultIncludes)
            .ToArrayAsync(cancellationToken)).OrderBy(x => x.DisplayTitleWithAbbr);
    }

    public async Task<JobTitle> ShowAsync(int jobTitleId, CancellationToken cancellationToken)
    {
        return (await repository.GetWithChildrenAsync<JobTitle>(
            x => x.Id == jobTitleId,
            cancellationToken,
            DefaultIncludes))!;
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