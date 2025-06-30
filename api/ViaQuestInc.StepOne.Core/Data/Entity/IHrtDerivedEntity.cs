namespace ViaQuestInc.StepOne.Core.Data.Entity;

public interface IHrtDerivedEntity
{
    Guid HrtId { get; set; }
    
    EntityStatuses EntityStatus { get; set; }
}