namespace ViaQuestInc.StepOne.Core.Data.Entity;

public interface IHrtSourceEntity<TStepOneEntity, TId>
    where TStepOneEntity : EntityBase<TId>
{
    EntityStatuses EntityStatus { get; set; }

    Task<TStepOneEntity> ToStepOneEntityAsync(
        IRepository<StepOneDbContext> repository,
        CancellationToken cancellationToken);
}