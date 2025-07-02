using Microsoft.EntityFrameworkCore;
using Serilog;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Synchronizers;

public class HrtSynchronizer<THrtEntity, TStepOneEntity, TId> : IDataSynchronizer
    where TStepOneEntity : EntityBase<TId>, IHrtDerivedEntity
    where THrtEntity : EntityBase<Guid>, IHrtSourceEntity<TStepOneEntity, TId>
{
    public async Task SyncAsync(
        IRepository<StepOneDbContext> stepOneRepository,
        IRepository<HrtDbContext> hrtRepository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken)
    {
        Log.Information(
            "  Synchronizing {StepOneEntity} entities with HR Tracker via {HrtEntity} entities",
            typeof(TStepOneEntity).Name,
            typeof(THrtEntity).Name);

        var allStepOneEntities = await stepOneRepository.All<TStepOneEntity>()
            .ToArrayAsync(cancellationToken);

        var allHrtEntities = await hrtRepository.All<THrtEntity>()
            .ToArrayAsync(cancellationToken);

        // Update status of StepOne entities based on HRT entities.
        foreach (var stepOneEntity in allStepOneEntities)
        {
            var hrtEntity = allHrtEntities.SingleOrDefault(x => x.Id == stepOneEntity.HrtId);

            stepOneEntity.EntityStatus = hrtEntity != null
                ? hrtEntity.EntityStatus
                : EntityStatuses.SoftDeleted;
        }

        await stepOneRepository.UpdateRangeAsync(allStepOneEntities, cancellationToken);

        // Create any missing StepOne entities that are active HRT entities.
        var newStepOneEntities = new List<TStepOneEntity>();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var hrtEntity in allHrtEntities.Where(x => x.EntityStatus == EntityStatuses.Active))
        {
            if (allStepOneEntities.Any(x => x.HrtId == hrtEntity.Id)) continue;

            var newStepOneEntity = await hrtEntity.ToStepOneEntityAsync(stepOneRepository, cancellationToken);

            newStepOneEntities.Add(newStepOneEntity);
        }

        await stepOneRepository.CreateRangeAsync(newStepOneEntities, cancellationToken);
    }
}