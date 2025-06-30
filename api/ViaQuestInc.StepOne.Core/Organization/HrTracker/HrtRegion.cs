using System.ComponentModel.DataAnnotations.Schema;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.HrTracker;

public class HrtRegion : EntityBase<Guid>,
    IHrtSourceEntity<Region, int>
{
    [Column("RegionID")]
    public override Guid Id { get; set; }

    [Column("RegionName")]
    public string Name { get; set; }
    
    [Column("Status")]
    public EntityStatuses EntityStatus { get; set; }
    
    public Task<Region> ToStepOneEntityAsync(
        IRepository<StepOneDbContext> repository,
        CancellationToken cancellationToken)
    {
        var region = new Region
        {
            Name = Name,
            HrtId = Id,
            EntityStatus = EntityStatus
        };

        return Task.FromResult(region);
    }
}