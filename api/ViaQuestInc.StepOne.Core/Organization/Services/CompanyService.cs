using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Services;

public class CompanyService(IRepository<StepOneDbContext> repository)
{
    public async Task<IEnumerable<Company>> IndexAsync(CancellationToken cancellationToken)
    {
        return await repository.Filter<Company>(x => x.EntityStatus == EntityStatuses.Active)
            .OrderBy(x => x.Name)
            .ToArrayAsync(cancellationToken);
    }
}