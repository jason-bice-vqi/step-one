using System.ComponentModel.DataAnnotations.Schema;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.HrTracker;

public class HrtBranch : EntityBase<Guid>,
    IHrtSourceEntity<Branch, int>
{
    [Column("BranchID")]
    public override Guid Id { get; set; }

    [Column("BranchName")]
    public string Name { get; set; }
    
    [Column("Status")]
    public EntityStatuses EntityStatus { get; set; }
    
    public Task<Branch> ToStepOneEntityAsync(
        IRepository<StepOneDbContext> repository,
        CancellationToken cancellationToken)
    {
        var branch = new Branch
        {
            Name = Name,
            HrtId = Id,
            EntityStatus = EntityStatus
        };

        return Task.FromResult(branch);
    }
}