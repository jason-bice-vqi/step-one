using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Organization.Hrt;

namespace ViaQuestInc.StepOne.Core.Organization.Synchronizers;

public class CompanySynchronizer : IDataSynchronizer
{
    public async Task SyncAsync(
        IRepository<StepOneDbContext> stepOneRepository,
        IRepository<HrtDbContext> hrtRepository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken)
    {
        var allStepOneCompanies = await stepOneRepository.All<Company>().ToArrayAsync(cancellationToken);
        var activeHrtCompanies = await hrtRepository.Filter<HrtCompany>(x => x.EntityStatus == EntityStatuses.Active)
            .ToArrayAsync(cancellationToken);

        // Update status of StepOne companies based on HRT companies.
        foreach (var stepOneCompany in allStepOneCompanies)
        {
            stepOneCompany.EntityStatus = activeHrtCompanies.Any(x => x.Id == stepOneCompany.HrtId)
                ? EntityStatuses.Active
                : EntityStatuses.SoftDeleted;
        }

        await stepOneRepository.UpdateRangeAsync(allStepOneCompanies, cancellationToken);

        // Create any missing StepOne companies.
        var newStepOneCompanies = new List<Company>();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var hrtCompany in activeHrtCompanies)
        {
            if (allStepOneCompanies.Any(x => x.HrtId == hrtCompany.Id)) continue;
            
            newStepOneCompanies.Add(new()
            {
                Name = hrtCompany.Name,
                Abbreviation = hrtCompany.Abbreviation,
                EntityStatus = EntityStatuses.Active,
                HrtId = hrtCompany.Id
            });
        }

        await stepOneRepository.CreateRangeAsync(newStepOneCompanies, cancellationToken);
    }
}