using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Services;

public class JobTitlesService(IRepository<StepOneDbContext> repository)
{
    public async Task<IEnumerable<JobTitle>> IndexAsync(int companyId, CancellationToken cancellationToken)
    {
        return await repository
            .Filter<JobTitle>(x => x.CompanyId == companyId && x.EntityStatus == EntityStatuses.Active)
            .OrderBy(x => x.Title)
            .ToArrayAsync(cancellationToken);
    }
}