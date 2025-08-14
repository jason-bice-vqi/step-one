using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Candidates;
using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Organization.Services;

public class JobTitleAliasService(IRepository<StepOneDbContext> repository)
{
    public async Task<IEnumerable<JobTitleAlias>> IndexAsync(int jobTitleId, CancellationToken cancellationToken)
    {
        return await repository.Filter<JobTitleAlias>(x => x.JobTitleId == jobTitleId)
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<JobTitleAlias> CreateAsync(
        int jobTitleId,
        string alias,
        CancellationToken cancellationToken)
    {
        var normalizedAlias = alias.ToUpper()
            .Trim();
        
        var newJobTitleAlias = new JobTitleAlias
        {
            Id = 0,
            JobTitleId = jobTitleId,
            Alias = normalizedAlias,
        };

        var jobTitleAlias = await repository.CreateAsync(newJobTitleAlias, cancellationToken);

        // TODO - determine what to do in reaction to a new alias.
        //
        // var candidatesWithAlias = await repository
        //     .Filter<Candidate>(x => x.AtsJobTitle == normalizedAlias && x.JobTitleId == null)
        //     .ToArrayAsync(cancellationToken);
        //
        // foreach (var candidateWithAlias in candidatesWithAlias)
        // {
        //     //candidateWithAlias.JobTitleId = jobTitleId;
        // }
        //
        // await repository.UpdateRangeAsync(candidatesWithAlias, cancellationToken);

        return jobTitleAlias;
    }

    public async Task<JobTitleAlias?> ShowAsync(int jobTitleAliasId, CancellationToken cancellationToken)
    {
        return await repository.FindAsync<JobTitleAlias>(jobTitleAliasId, cancellationToken);
    }

    public async Task DeleteAsync(JobTitleAlias jobTitleAlias, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(jobTitleAlias, cancellationToken);
    }
}